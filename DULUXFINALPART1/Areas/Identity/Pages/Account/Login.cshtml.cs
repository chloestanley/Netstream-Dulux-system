using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

public class LoginModel : PageModel
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<LoginModel> _logger;

    public LoginModel(
        SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager,
        ILogger<LoginModel> logger)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _logger = logger;
    }

    [BindProperty]
    public InputModel Input { get; set; }

    public IList<AuthenticationScheme> ExternalLogins { get; set; }

    public string ReturnUrl { get; set; }

    [TempData]
    public string ErrorMessage { get; set; }

    public class InputModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        //[Display(Name = "Remember me?")]
        //public bool RememberMe { get; set; }
    }

    public async Task OnGetAsync(string returnUrl = null)
    {
        if (!string.IsNullOrEmpty(ErrorMessage))
        {
            ModelState.AddModelError(string.Empty, ErrorMessage);
        }

        returnUrl ??= Url.Content("~/");

        await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

        ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

        ReturnUrl = returnUrl;
    }

    public async Task<IActionResult> OnPostAsync(string returnUrl = null)
    {
        returnUrl ??= Url.Content("~/");

        ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

        if (ModelState.IsValid)
        {

            var result = await _signInManager.PasswordSignInAsync(
            Input.Email,
            Input.Password,
            isPersistent: false, // 👈 ALWAYS false
            lockoutOnFailure: false
        );

            if (result.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(Input.Email);

                // Convert UTC time to Durban time
                TimeZoneInfo durbanZone = TimeZoneInfo.FindSystemTimeZoneById("South Africa Standard Time");
                DateTime durbanTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, durbanZone);
                user.LastLoginTime = durbanTime;

                await _userManager.UpdateAsync(user);

                _logger.LogInformation("User logged in.");

                // Redirect based on role
                if (await _userManager.IsInRoleAsync(user, "Admin") || await _userManager.IsInRoleAsync(user, "ControlRoom"))
                {
                    return LocalRedirect("~/Home/Index");
                }
                else if (await _userManager.IsInRoleAsync(user, "ScanOperator"))
                {
                    return LocalRedirect("~/Scan_Image/Create");

                }
                else if (await _userManager.IsInRoleAsync(user, "Guard"))
                {
                    return LocalRedirect("~/Scan_Image/Note");

                }
                else
                {
                    // fallback if role not recognized
                    return LocalRedirect("~/");
                }

            }
            if (result.RequiresTwoFactor)
            {
                return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl});
            }
            if (result.IsLockedOut)
            {
                _logger.LogWarning("User account locked out.");
                return RedirectToPage("./Lockout");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return Page();
            }
        }

        return Page();
    }
}
