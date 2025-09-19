using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

public class CustomSignInManager : SignInManager<ApplicationUser>
{
    public CustomSignInManager(
        UserManager<ApplicationUser> userManager,
        IHttpContextAccessor contextAccessor,
        IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory,
        IOptions<IdentityOptions> optionsAccessor,
        ILogger<SignInManager<ApplicationUser>> logger,
        IAuthenticationSchemeProvider schemes,
        IUserConfirmation<ApplicationUser> confirmation)
        : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, confirmation)
    {
    }

    public override async Task SignInAsync(ApplicationUser user, bool isPersistent, string authenticationMethod = null)
    {
        user.LastLoginTime = DateTime.UtcNow;
        await UserManager.UpdateAsync(user);
        await base.SignInAsync(user, isPersistent, authenticationMethod);
    }
}
