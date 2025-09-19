using Azure.Storage.Blobs;
using DULUXFINALPART1.Data;
using DULUXFINALPART1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

[Authorize(Roles = "ScanOperator")]
public class Scan_ImageController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly ComputerVisionClient _computerVisionClient;

    public Scan_ImageController(ApplicationDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;

        string endpoint = _configuration["AzureComputerVision:Endpoint"];
        string apiKey = _configuration["AzureComputerVision:ApiKey"];

        if (string.IsNullOrEmpty(endpoint) || string.IsNullOrEmpty(apiKey))
        {
            throw new InvalidOperationException("Azure API endpoint or API key is missing.");
        }

        _computerVisionClient = new ComputerVisionClient(new ApiKeyServiceClientCredentials(apiKey))
        {
            Endpoint = endpoint
        };
    }

    public async Task<IActionResult> Index()
    {
        var scanImages = await _context.Scan_Images.ToListAsync();
        var sortedScanImages = scanImages.OrderByDescending(item => item.Id).ToList();
        var highestIds = sortedScanImages.Take(1).Select(item => item.Id).ToList();

        string containerName = _configuration["AzureBlobStorage:ContainerName"];
        foreach (var image in scanImages)
        {
            string blobUriWithSas = GenerateSasUri(containerName, $"{image.Id}.png");
            image.ImageUri = blobUriWithSas;
        }

        ViewBag.HighestIds = highestIds;
        return View(scanImages);
    }

    public string GenerateSasUri(string containerName, string blobName)
    {
        string connectionString = _configuration.GetConnectionString("BlobStorageConnection");
        BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
        BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
        BlobClient blobClient = containerClient.GetBlobClient(blobName);

        string sasToken = _configuration["AzureBlobStorage:SasToken"];
        string sas = sasToken.StartsWith("?") ? sasToken.Substring(1) : sasToken;

        return $"{blobClient.Uri}?{sas}";
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,ContentType,Extracted_Text,Shipment,IsSelected,DeliveryNo1,DeliveryNo2,DeliveryNo3,DeliveryNo4,DeliveryNo5,TotalPC,TotalVolume,Carrier")] Scan_Image scan_Image, IFormFile imageFile)
    {
        if (imageFile == null || imageFile.Length == 0)
        {
            ModelState.AddModelError("", "Please upload a valid image.");
            return View(scan_Image);
        }

        using var memoryStream = new MemoryStream();
        await imageFile.CopyToAsync(memoryStream);
        scan_Image.ContentType = imageFile.ContentType;

        try
        {
            scan_Image.CreatedAt = DateTime.Now;
            _context.Add(scan_Image);
            await _context.SaveChangesAsync();

            // Upload image to Azure Blob
            string containerName = _configuration["AzureBlobStorage:ContainerName"];
            string connectionString = _configuration.GetConnectionString("BlobStorageConnection");
            string blobUri = await UploadToBlobAsync(memoryStream, scan_Image.Id, connectionString, containerName);
            scan_Image.ImageUri = blobUri;

            // OCR with Azure
            memoryStream.Position = 0;
            var extractedText = await PerformOcrWithAzure(memoryStream);
            scan_Image.Extracted_Text = extractedText ?? string.Empty;

            // Post-processing text
            string[] excludedKeywords = { "Route:", "Please Print Name", "Date", "Security", "Vehicle Reg", "Transporter", "Receiver" };
            var lines = new List<string>();
            bool stopAddingText = false;

            foreach (var line in extractedText.Split('\n'))
            {
                if (stopAddingText) break;
                if (excludedKeywords.Any(keyword => line.Trim().StartsWith(keyword, StringComparison.OrdinalIgnoreCase)))
                    continue;

                lines.Add(line);
                if (line.Contains("Route:", StringComparison.OrdinalIgnoreCase)) stopAddingText = true;
            }

            scan_Image.Extracted_Text = string.Join("\n", lines);

            // Shipment extraction
            string shipmentLine = lines.FirstOrDefault(line => line.Contains("Shipment"));
            if (!string.IsNullOrEmpty(shipmentLine))
            {
                int colonIndex = shipmentLine.IndexOf(':');
                scan_Image.Shipment = (colonIndex != -1 && colonIndex + 1 < shipmentLine.Length)
                    ? shipmentLine.Substring(colonIndex + 1).Trim()
                    : shipmentLine.Trim();
            }

            // Prevent duplicate shipment
            if (await _context.Scan_Images.AnyAsync(si => si.Shipment == scan_Image.Shipment))
            {
                ViewBag.ErrorMessage = "A shipment with this number already exists.";
                return View(scan_Image);
            }

            // Extract delivery numbers
            var matches = Regex.Matches(extractedText, @"\b\d{10}\b");
            if (matches.Count >= 1) scan_Image.DeliveryNo1 = matches[0].Value;
            if (matches.Count >= 2) scan_Image.DeliveryNo2 = matches[1].Value;
            if (matches.Count >= 3) scan_Image.DeliveryNo3 = matches[2].Value;
            if (matches.Count >= 4) scan_Image.DeliveryNo4 = matches[3].Value;
            if (matches.Count >= 5) scan_Image.DeliveryNo5 = matches[4].Value;

            // Extract totals
            var linesArray = extractedText.Split('\n');
            for (int i = 0; i < linesArray.Length; i++)
            {
                var line = linesArray[i];
                if (line.Contains("Total PC", StringComparison.OrdinalIgnoreCase))
                {
                    string nextLine = (i + 1 < linesArray.Length) ? linesArray[i + 1].Trim() : null;
                    scan_Image.TotalPC = line.Contains(":")
                        ? line.Substring(line.IndexOf(':') + 1).Trim()
                        : nextLine;
                }
                if (line.Contains("Total Volume", StringComparison.OrdinalIgnoreCase))
                {
                    string nextLine = (i + 1 < linesArray.Length) ? linesArray[i + 1].Trim() : null;
                    scan_Image.TotalVolume = line.Contains(":")
                        ? line.Substring(line.IndexOf(':') + 1).Trim()
                        : nextLine;
                }
            }

            // Extract carrier
            var carrierLine = extractedText.Split('\n')
                .FirstOrDefault(l => l.Trim().StartsWith("Carrier", StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(carrierLine))
            {
                int colonIndex = carrierLine.IndexOf(':');
                scan_Image.Carrier = colonIndex != -1 ? carrierLine.Substring(colonIndex + 1).Trim() : carrierLine.Trim();
            }

            _context.Update(scan_Image);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            ViewBag.ErrorMessage = $"Error processing image: {ex.Message}";
            return View(scan_Image);
        }

        HttpContext.Session.SetString("TimerStart", DateTime.Now.ToString("o"));
        return RedirectToAction(nameof(Index));
    }

    public async Task<string> UploadToBlobAsync(MemoryStream stream, int id, string connectionString, string containerName)
    {
        string blobName = $"{id}.png";
        BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
        BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
        await containerClient.CreateIfNotExistsAsync();

        BlobClient blobClient = containerClient.GetBlobClient(blobName);
        stream.Position = 0;
        await blobClient.UploadAsync(stream, overwrite: true);

        return blobClient.Uri.ToString();
    }

    private async Task<string> PerformOcrWithAzure(Stream imageStream)
    {
        imageStream.Position = 0;

        // Use Read API (new OCR)
        var textHeaders = await _computerVisionClient.ReadInStreamAsync(imageStream);
        string operationLocation = textHeaders.OperationLocation;
        string operationId = operationLocation.Split('/').Last();

        // Poll for results
        ReadOperationResult results;
        do
        {
            await Task.Delay(1000);
            results = await _computerVisionClient.GetReadResultAsync(Guid.Parse(operationId));
        }
        while (results.Status == OperationStatusCodes.Running || results.Status == OperationStatusCodes.NotStarted);

        if (results.Status == OperationStatusCodes.Succeeded)
        {
            var textResults = results.AnalyzeResult.ReadResults;
            return string.Join("\n", textResults.SelectMany(r => r.Lines.Select(l => l.Text)));
        }

        return string.Empty;
    }

    [HttpPost]
    public async Task<IActionResult> UpdateSelected(List<int> selectedIds)
    {
        if (selectedIds == null || !selectedIds.Any())
            return RedirectToAction("Index");

        string? startTimeStr = HttpContext.Session.GetString("TimerStart");
        DateTime startTime;
        bool hasTimer = DateTime.TryParse(startTimeStr, out startTime);

        foreach (var id in selectedIds)
        {
            var scanImage = await _context.Scan_Images.FindAsync(id);
            if (scanImage != null)
            {
                scanImage.IsSelected = true;
                if (hasTimer)
                {
                    TimeSpan duration = DateTime.Now - startTime;
                    scanImage.TimeTakenSeconds = (int)duration.TotalSeconds;
                }
                _context.Update(scanImage);
            }
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Create));
    }

    [AllowAnonymous]
    public async Task<IActionResult> Note()
    {
        var selectedImages = await _context.Scan_Images
            .Where(si => si.IsSelected)
            .OrderByDescending(si => si.Id)
            .ToListAsync();

        return View(selectedImages);
    }

    public async Task<IActionResult> Time()
    {
        var shipments = await _context.Scan_Images
            .Where(s => !string.IsNullOrEmpty(s.Shipment))
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync();

        return View(shipments);
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var image = await _context.Scan_Images.FindAsync(id);
        if (image == null) return NotFound();
        return View(image);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var image = await _context.Scan_Images.FindAsync(id);
        if (image != null)
        {
            _context.Scan_Images.Remove(image);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Create));
    }

    public async Task<IActionResult> TrackShipment(string shipmentNumber)
    {
        string apiUrl = $"https://yourdomain.com/api/shipment/track/{shipmentNumber}";
        using var client = new HttpClient();
        var response = await client.GetAsync(apiUrl);

        if (!response.IsSuccessStatusCode)
        {
            ViewBag.ErrorMessage = "Error tracking the shipment. Please try again later.";
            return View();
        }

        var result = await response.Content.ReadAsStringAsync();
        var shipmentDetails = JsonConvert.DeserializeObject<dynamic>(result);
        return View(shipmentDetails);
    }

    [Authorize(Roles = "ControlRoom")]
    [AllowAnonymous]
    public async Task<IActionResult> Reports(string searchTerm, int? month)
    {
        var query = _context.Scan_Images.AsQueryable();
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(s =>
                (!string.IsNullOrEmpty(s.Shipment) && s.Shipment.Contains(searchTerm)) ||
                (!string.IsNullOrEmpty(s.Extracted_Text) && s.Extracted_Text.Contains(searchTerm)));
        }
        if (month.HasValue) query = query.Where(s => s.CreatedAt.Month == month.Value);

        var scanImages = await query.OrderByDescending(s => s.CreatedAt).ToListAsync();
        return View("Reports", scanImages);
    }

    [Authorize(Roles = "ControlRoom")]
    [AllowAnonymous]
    public async Task<IActionResult> LCNReportPrint(string searchTerm, int? month)
    {
        var query = _context.Scan_Images.AsQueryable();
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(s =>
                (!string.IsNullOrEmpty(s.Shipment) && s.Shipment.Contains(searchTerm)) ||
                (!string.IsNullOrEmpty(s.Extracted_Text) && s.Extracted_Text.Contains(searchTerm)));
        }
        if (month.HasValue) query = query.Where(s => s.CreatedAt.Month == month.Value);

        var scanImages = await query.OrderByDescending(s => s.CreatedAt).ToListAsync();
        return View("LCNReportPrint", scanImages);
    }

    [Authorize(Roles = "ControlRoom")]
    [AllowAnonymous]
    public IActionResult DownloadCSV(string searchTerm, int? month)
    {
        var query = _context.Scan_Images.AsQueryable();
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(s =>
                (!string.IsNullOrEmpty(s.Shipment) && s.Shipment.Contains(searchTerm)) ||
                (!string.IsNullOrEmpty(s.Extracted_Text) && s.Extracted_Text.Contains(searchTerm)));
        }
        if (month.HasValue) query = query.Where(s => s.CreatedAt.Month == month.Value);

        var scanImages = query.OrderByDescending(s => s.CreatedAt).ToList();
        var csv = GenerateCsv(scanImages);

        var fileName = "LCNReport.csv";
        var fileBytes = Encoding.UTF8.GetBytes(csv);
        return File(fileBytes, "text/csv", fileName);
    }

    private string GenerateCsv(List<Scan_Image> scanImages)
    {
        var csv = new StringBuilder();
        csv.AppendLine("Shipment,Carrier,Total PC,Total Volume,Created At");

        foreach (var item in scanImages)
        {
            csv.AppendLine($"{item.Shipment},{item.Carrier},{item.TotalPC},{item.TotalVolume},{item.CreatedAt:yyyy-MM-dd HH:mm}");
        }

        return csv.ToString();
    }
}
