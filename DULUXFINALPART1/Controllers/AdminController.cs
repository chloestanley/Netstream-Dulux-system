using IronPdf;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using DULUXFINALPART1.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

public class AdminController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;

    public AdminController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> DownloadAllLoginDataAsPdf()
    {
        var users = await _userManager.Users.ToListAsync();

        var htmlContent = @"
            <html>
            <head>
                <style>
                    body { font-family: Arial, sans-serif; margin: 40px; }
                    h1 { color: #2c3e50; }
                    table { width: 100%; border-collapse: collapse; margin-top: 20px; }
                    th, td { border: 1px solid #ccc; padding: 8px; text-align: left; }
                    th { background-color: #f2f2f2; }
                    tr:nth-child(even) { background-color: #fafafa; }
                </style>
            </head>
            <body>
                <h1>All Users Login Data</h1>
                <table>
                    <thead>
                        <tr>
                            <th>User ID</th>
                            <th>User Name</th>
                            <th>Email</th>
                            <th>Last Login Time</th>
                            <th>Last Logout Time</th>
                        </tr>
                    </thead>
                    <tbody>";

        foreach (var user in users)
        {
            htmlContent += $@"
                        <tr>
                            <td>{user.Id}</td>
                            <td>{user.UserName}</td>
                            <td>{user.Email}</td>
                            <td>{user.LastLoginTime?.ToString("g") ?? "N/A"}</td>
                            <td>{user.LastLogoutTime?.ToString("g") ?? "N/A"}</td>
                        </tr>";
        }

        htmlContent += @"
                    </tbody>
                </table>
            </body>
            </html>";

        var Renderer = new HtmlToPdf();
        var pdf = Renderer.RenderHtmlAsPdf(htmlContent);
        return File(pdf.BinaryData, "application/pdf", "AllUserLoginData.pdf");
    }
    [HttpGet]
    public async Task<IActionResult> DownloadAllUsersAsCsv()
    {
        var users = await _userManager.Users.ToListAsync();

        // Prepare CSV content
        var csvContent = "UserName,Email,LastLoginTime,LastLogoutTime\n";

        foreach (var user in users)
        {
            csvContent += $"{user.UserName},{user.Email},{user.LastLoginTime},{user.LastLogoutTime}\n";
        }

        var fileBytes = System.Text.Encoding.UTF8.GetBytes(csvContent);
        return File(fileBytes, "text/csv", "AllUsersData.csv");
    }

    [HttpGet]
    public async Task<IActionResult> DownloadPersonalData(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return NotFound();

        var csvContent = $"UserName,Email,LastLoginTime,LastLogoutTime\n" +
                         $"{user.UserName},{user.Email},{user.LastLoginTime},{user.LastLogoutTime}";
        var fileBytes = System.Text.Encoding.UTF8.GetBytes(csvContent);
        return File(fileBytes, "text/csv", "PersonalData.csv");
    }

    public async Task<IActionResult> Index(string searchString)
    {
        var users = _userManager.Users.AsQueryable();

        if (!string.IsNullOrEmpty(searchString))
        {
            users = users.Where(u => u.UserName.Contains(searchString) || u.Email.Contains(searchString));
        }

        return View(await users.ToListAsync());
    }
}
