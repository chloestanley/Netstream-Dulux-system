using Azure.Storage.Blobs;
using DULUXFINALPART1.Data;
using DULUXFINALPART1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class GuardsController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;

    public GuardsController(ApplicationDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    // GET: Guards/Index

    
    public async Task<IActionResult> Index()
    {
        var guards = _context.Guards.Include(g => g.ScanImage);
        return View(await guards.ToListAsync());
    }

    //[HttpGet]
    //public IActionResult Create()
    //{
    //    var highestIdRecord = _context.Scan_Images
    //        .OrderByDescending(s => s.Id)
    //        .FirstOrDefault();

    //    ViewBag.Shipments = new SelectList(new[] { highestIdRecord }, "Id", "Shipment");
    //    return View();
    //}
    [HttpGet]
    public IActionResult Create()
    {
        // Fetch all Scan_Images from the database
        var shipments = _context.Scan_Images
            .OrderByDescending(s => s.Id) // Optional: newest first
            .ToList();

        // Populate the dropdown with ALL shipments
        ViewBag.Shipments = new SelectList(shipments, "Id", "Shipment");

        return View();
    }


    // GET: Guards/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(GuardsPage guardsPage, List<IFormFile> imageFiles)
    {
        if (!ModelState.IsValid)
        {
            // Repopulate dropdown
            var shipments = _context.Scan_Images
                .OrderByDescending(s => s.Id)
                .ToList();
            ViewBag.Shipments = new SelectList(shipments, "Id", "Shipment", guardsPage.ScanImageId);

            return View(guardsPage);
        }

        try
        {
            // Initial save to get GuardsId
            _context.Add(guardsPage);
            await _context.SaveChangesAsync();

            Console.WriteLine("Signature received: " + guardsPage.Signature);

            var containerName = _configuration["AzureBlobStorage:ContainerName"];
            var connectionString = _configuration.GetConnectionString("BlobStorageConnection");

            for (int i = 0; i < imageFiles.Count && i < 4; i++)
            {
                var file = imageFiles[i];
                if (file != null && file.Length > 0)
                {
                    string blobName = $"guards/{guardsPage.GuardsId}_image{i + 1}.png";
                    var imageUrl = await UploadToBlobAsync(file, blobName, connectionString, containerName);

                    switch (i)
                    {
                        case 0: guardsPage.ImageUrl1 = imageUrl; break;
                        case 1: guardsPage.ImageUrl2 = imageUrl; break;
                        case 2: guardsPage.ImageUrl3 = imageUrl; break;
                        case 3: guardsPage.ImageUrl4 = imageUrl; break;
                    }
                }
            }

            _context.Update(guardsPage);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            // Repopulate dropdown before returning view
            var shipments = _context.Scan_Images
                .OrderByDescending(s => s.Id)
                .ToList();
            ViewBag.Shipments = new SelectList(shipments, "Id", "Shipment", guardsPage.ScanImageId);

            ViewBag.ErrorMessage = $"Error saving guard entry: {ex.Message}";
            return View(guardsPage);
        }

        return RedirectToAction("Create");
    }



    private async Task<string> UploadToBlobAsync(IFormFile file, string blobName, string connectionString, string containerName)
    {
        var blobServiceClient = new BlobServiceClient(connectionString);
        var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
        await containerClient.CreateIfNotExistsAsync();
        var blobClient = containerClient.GetBlobClient(blobName);

        using var stream = file.OpenReadStream();
        await blobClient.UploadAsync(stream, overwrite: true);

        return blobClient.Uri.ToString();
    }

    // Optional: Control Room Search (Post)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Search(string searchQuery)
    {
        if (string.IsNullOrWhiteSpace(searchQuery))
        {
            TempData["Message"] = "Please enter a valid search query.";
            return View(new List<GuardsPage>());
        }

        var query = searchQuery.Trim().ToLower();

        var results = await _context.Guards
            .Include(g => g.ScanImage) // Include related ScanImage
            .Where(g =>
                g.DateTime.ToString().ToLower().Contains(query) ||
                g.Guard_SecurityCard_No.ToString().Contains(query) ||
                g.Driver_Name.ToLower().Contains(query) ||
                g.Guard_Name.ToLower().Contains(query) ||
                (g.ScanImage != null && g.ScanImage.Shipment.ToLower().Contains(query)) // ✅ Search Shipment number
            )
            .ToListAsync();

        if (!results.Any())
        {
            TempData["Message"] = "No records matched your search.";
        }

        return View(results);
    }

    // Optional: GET Search view
    public IActionResult Search()
    {
        return View(new List<GuardsPage>());
    }

    public IActionResult Links()
    {
        return View();
    }

    public async Task<IActionResult> ReturnsReport()
    {
        var returns = await _context.Returns
            .Include(r => r.ScanImage) // Include related data like ScanImage
            .ToListAsync();

        return View(returns); // Return the returns to the ReturnsReport view
    }
    [HttpGet]
    public async Task<IActionResult> DownloadAllReturnsAsCsv()
    {
        var returns = await _context.Returns.Include(r => r.ScanImage).ToListAsync();

        // Prepare CSV content
        var csvContent = "DeliveryNumber,Comments,Shipment\n";

        foreach (var item in returns)
        {
            csvContent += $"{item.DeliveryNumber},{item.Comments},{item.ScanImage?.Shipment}\n";
        }

        var fileBytes = System.Text.Encoding.UTF8.GetBytes(csvContent);
        return File(fileBytes, "text/csv", "AllReturnsData.csv");
    }
    public IActionResult GuardReports()
    {
        // Fetch the data you want to display in the report
        var guardsList = _context.Guards.ToList();

        // Pass the data to the view
        return View(guardsList);
    }
    public async Task<IActionResult> DownloadAllGuardsAsCsv()
    {
        var guards = await _context.Guards.ToListAsync();
        var csv = new StringBuilder();

        // Add header without image URLs
        csv.AppendLine("Guard Name, Guard Security Card No, Driver Name, Driver Security Card No, DateTime, Acceptance, SendToControlRoom");

        // Add data rows, excluding the image URLs
        foreach (var guard in guards)
        {
            csv.AppendLine($"{guard.Guard_Name},{guard.Guard_SecurityCard_No},{guard.Driver_Name},{guard.Driver_SecurityCard_No},{guard.DateTime},{guard.Acceptance},{guard.SendToControlRoom}");
        }

        // Return CSV file as a download
        var fileName = "GuardReports.csv";
        return File(Encoding.UTF8.GetBytes(csv.ToString()), "text/csv", fileName);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Confirm(int id, bool confirm)
    {
    var record = await _context.Guards.FindAsync(id);
    if (record == null)
    {
        TempData["Message"] = "Record not found.";
        return RedirectToAction("Search");
    }

    // Block re-confirmation
    if (record.IsConfirmed || record.IsRejected)
    {
        TempData["Message"] = "Confirmation already submitted.";
        return RedirectToAction("Search");
    }

    if (confirm)
        record.IsConfirmed = true;
    else
        record.IsRejected = true;

    _context.Update(record);
    await _context.SaveChangesAsync();

    TempData["Message"] = "Confirmation saved successfully.";
    return RedirectToAction("Search");
}
    public async Task<IActionResult> GuardNotificationNote()
    {
        var guards = await _context.Guards
            .Include(g => g.ScanImage) // Make sure this is included
            .Where(g => g.SendToControlRoom)
            .OrderByDescending(g => g.DateTime)
            .ToListAsync();

        return View(guards);
    }



}
