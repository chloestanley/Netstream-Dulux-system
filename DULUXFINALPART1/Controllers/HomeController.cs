using System.Diagnostics;
using System.Globalization;
using System.Text.Json;
using DULUXFINALPART1.Data;
using DULUXFINALPART1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DULUXFINALPART1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        
        public IActionResult Index()
        {
            var now = DateTime.Now;

            // ?? Daily data for the last 7 days
            var dailyData = _context.Scan_Images
                .Where(x => x.CreatedAt >= now.AddDays(-7))
                .GroupBy(x => x.CreatedAt.Date)
                .OrderBy(g => g.Key)
                .Select(g => new
                {
                    Day = g.Key.ToString("ddd"),
                    Count = g.Count()
                })
                .ToList();

            // ?? Monthly data for the current year
            var monthlyData = _context.Scan_Images
                .Where(x => x.CreatedAt.Year == now.Year)
                .GroupBy(x => x.CreatedAt.Month)
                .OrderBy(g => g.Key)
                .Select(g => new
                {
                    Month = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(g.Key),
                    Count = g.Count()
                })
                .ToList();

            // ?? Last LCN capture per day
            var dailyLCNDetails = _context.Scan_Images
                .Where(x => x.CreatedAt >= now.AddDays(-7))
                .GroupBy(x => x.CreatedAt.Date)
                .OrderBy(g => g.Key)
                .Select(g => g.OrderByDescending(x => x.CreatedAt).FirstOrDefault())
                .ToList();

            // ?? Bubble Chart: Frequency by hour & day of week
            // ?? Bubble Chart: Frequency by hour & day of week (client-side LINQ)
            var bubbleChartRawData = _context.Scan_Images
                .Where(x => x.CreatedAt >= now.AddDays(-7))
                .AsEnumerable() // ?? Move evaluation to client-side
                .GroupBy(x => new
                {
                    Hour = x.CreatedAt.Hour,
                    DayOfWeek = (int)x.CreatedAt.DayOfWeek
                })
                .Select(g => new
                {
                    x = g.Key.Hour,        // x-axis: hour
                    y = g.Key.DayOfWeek,   // y-axis: day of week (0 = Sunday)
                    r = g.Count()          // radius: count
                })
                .ToList();


            // ?? ViewBag assignments for charts
            ViewBag.LCNSPerDayLabels = JsonSerializer.Serialize(dailyData.Select(d => d.Day));
            ViewBag.LCNSPerDayValues = JsonSerializer.Serialize(dailyData.Select(d => d.Count));

            ViewBag.LCNSPerMonthLabels = JsonSerializer.Serialize(monthlyData.Select(m => m.Month));
            ViewBag.LCNSPerMonthValues = JsonSerializer.Serialize(monthlyData.Select(m => m.Count));

            ViewBag.LastLCNDayLabels = JsonSerializer.Serialize(dailyLCNDetails.Select(d => d?.CreatedAt.ToString("ddd") ?? "--"));
            ViewBag.LastLCNTimeValues = JsonSerializer.Serialize(dailyLCNDetails.Select(d => d?.CreatedAt.ToString("HH:mm") ?? "--"));

            ViewBag.BubbleChartData = JsonSerializer.Serialize(bubbleChartRawData);

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
        
        public IActionResult LCN()
        {
            var scanImages = _context.Scan_Images
                .OrderByDescending(s => s.CreatedAt)
                .ToList();

            return View(scanImages); // Pass to Privacy view
        }
    }
}
