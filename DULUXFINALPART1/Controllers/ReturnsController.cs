using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DULUXFINALPART1.Data;
using DULUXFINALPART1.Models;

namespace DULUXFINALPART1.Controllers
{
    public class ReturnsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReturnsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Returns
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Returns
                .Include(r => r.ScanImage); // Include ScanImage for shipment info

            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Returns/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var @return = await _context.Returns
                .Include(r => r.ScanImage)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (@return == null) return NotFound();

            return View(@return);
        }

        // GET: Returns/Create
        public IActionResult Create()
        {
            ViewData["ScanImageId"] = new SelectList(_context.Scan_Images, "Id", "Shipment");
            return View();
        }

        // POST: Returns/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ScanImageId,Comments")] Return @return, string SelectedDeliveries)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(SelectedDeliveries))
                {
                    var deliveryList = SelectedDeliveries.Split(',', StringSplitOptions.RemoveEmptyEntries);

                    foreach (var deliveryNumber in deliveryList)
                    {
                        var existingReturn = _context.Returns
                            .Any(r => r.DeliveryNumber == deliveryNumber);

                        if (existingReturn)
                        {
                            ModelState.AddModelError("", $"Delivery number {deliveryNumber} has already been used.");
                            return View(@return);
                        }

                        var newReturn = new Return
                        {
                            ScanImageId = @return.ScanImageId,
                            Comments = @return.Comments,
                            DeliveryNumber = deliveryNumber
                        };

                        newReturn.SetReturnCreatedAt();

                        _context.Add(newReturn);
                    }

                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError("", "Please select at least one delivery number.");
            }

            ViewData["ScanImageId"] = new SelectList(_context.Scan_Images, "Id", "Shipment", @return.ScanImageId);
            return View(@return);
        }

        // GET: Returns/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var @return = await _context.Returns.FindAsync(id);
            if (@return == null) return NotFound();

            ViewData["ScanImageId"] = new SelectList(_context.Scan_Images, "Id", "Shipment", @return.ScanImageId);
            return View(@return);
        }

        // POST: Returns/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ScanImageId,DeliveryNumber,Comments")] Return @return)
        {
            if (id != @return.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@return);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReturnExists(@return.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["ScanImageId"] = new SelectList(_context.Scan_Images, "Id", "Shipment", @return.ScanImageId);
            return View(@return);
        }

        // GET: Returns/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var @return = await _context.Returns
                .Include(r => r.ScanImage)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (@return == null) return NotFound();

            return View(@return);
        }

        // POST: Returns/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @return = await _context.Returns.FindAsync(id);
            if (@return != null)
            {
                _context.Returns.Remove(@return);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ReturnExists(int id)
        {
            return _context.Returns.Any(e => e.Id == id);
        }

        // AJAX endpoint for dynamic delivery number loading
        public JsonResult GetDeliveryNumbers(int scanImageId)
        {
            var scanImage = _context.Scan_Images.FirstOrDefault(s => s.Id == scanImageId);

            if (scanImage != null)
            {
                var deliveryNumbers = new List<string>();

                if (!string.IsNullOrEmpty(scanImage.DeliveryNo1)) deliveryNumbers.Add(scanImage.DeliveryNo1);
                if (!string.IsNullOrEmpty(scanImage.DeliveryNo2)) deliveryNumbers.Add(scanImage.DeliveryNo2);
                if (!string.IsNullOrEmpty(scanImage.DeliveryNo3)) deliveryNumbers.Add(scanImage.DeliveryNo3);
                if (!string.IsNullOrEmpty(scanImage.DeliveryNo4)) deliveryNumbers.Add(scanImage.DeliveryNo4);
                if (!string.IsNullOrEmpty(scanImage.DeliveryNo5)) deliveryNumbers.Add(scanImage.DeliveryNo5);

                var usedDeliveryNumbers = _context.Returns
                    .Where(r => r.ScanImageId == scanImageId)
                    .Select(r => r.DeliveryNumber)
                    .ToList();

                var availableDeliveryNumbers = deliveryNumbers
                    .Where(d => !usedDeliveryNumbers.Contains(d))
                    .ToList();

                return Json(availableDeliveryNumbers);
            }

            return Json(new List<string>());
        }

        // GET: Returns/Report
       

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

    }
}
