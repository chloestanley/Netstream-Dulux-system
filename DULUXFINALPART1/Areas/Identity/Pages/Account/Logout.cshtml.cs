using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

public class LogoutModel : PageModel
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<LogoutModel> _logger;

    public LogoutModel(
        SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager,
        ILogger<LogoutModel> logger)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<IActionResult> OnPost(string returnUrl = null)
    {
        if (User.Identity.IsAuthenticated)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                user.LastLogoutTime = GetDurbanTime(); // Durban time
                await _userManager.UpdateAsync(user);
            }
        }

        await _signInManager.SignOutAsync();
        _logger.LogInformation("User logged out.");

        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
        {
            return LocalRedirect(returnUrl);
        }

        return RedirectToPage("/Index");
    }

    private DateTime GetDurbanTime()
    {
        TimeZoneInfo durbanZone = TimeZoneInfo.FindSystemTimeZoneById("South Africa Standard Time");
        return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, durbanZone);
    }
}