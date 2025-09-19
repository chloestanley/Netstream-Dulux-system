using Microsoft.Extensions.DependencyInjection;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Microsoft.Extensions.Configuration;
using DULUXFINALPART1.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

// Configure database connection
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Configure Identity with custom ApplicationUser class
builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>();

// Add the custom SignInManager
builder.Services.AddScoped<SignInManager<ApplicationUser>, CustomSignInManager>();

// Add MVC and Razor Pages
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Add session support
builder.Services.AddSession();

// Add Azure Computer Vision client
builder.Services.AddSingleton<ComputerVisionClient>(serviceProvider =>
{
    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
    var endpoint = configuration["AzureComputerVision:Endpoint"];
    var apiKey = configuration["AzureComputerVision:ApiKey"];

    if (string.IsNullOrEmpty(endpoint) || string.IsNullOrEmpty(apiKey))
    {
        throw new InvalidOperationException("Azure API endpoint or API key is missing.");
    }

    return new ComputerVisionClient(new ApiKeyServiceClientCredentials(apiKey))
    {
        Endpoint = endpoint
    };
});

// Enable CORS for Tidio chatbot
builder.Services.AddCors(options =>
{
    options.AddPolicy("TidioPolicy", policy =>
    {
        policy.WithOrigins("https://www.tidio.com", "https://code.tidio.co")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// Create default roles and assign users
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

    await SeedRolesAsync(services);
}

// Configure middleware
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Apply CORS for chatbot calls
app.UseCors("TidioPolicy");

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

// Route endpoints
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();

// Role and user setup methods
async Task SeedRolesAsync(IServiceProvider serviceProvider)
{
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    // Add all roles used in your Razor view
    string[] roles = { "Admin", "User", "ControlRoom", "Guard", "ScanOperator" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    // Optionally: Seed users for each role
    await SeedUserWithRole(userManager, "admin@example.com", "Admin123!", "Admin");
    await SeedUserWithRole(userManager, "control@example.com", "Tushar1?", "ControlRoom");
    await SeedUserWithRole(userManager, "guard@example.com", "Tushar1!!", "Guard");
    await SeedUserWithRole(userManager, "scan@example.com", "Tushar1@", "ScanOperator");
}

async Task SeedUserWithRole(UserManager<ApplicationUser> userManager, string email, string password, string role)
{
    var user = await userManager.FindByEmailAsync(email);
    if (user == null)
    {
        user = new ApplicationUser
        {
            UserName = email,
            Email = email,
            EmailConfirmed = true
        };
        var result = await userManager.CreateAsync(user, password);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(user, role);
        }
        else
        {
            foreach (var error in result.Errors)
            {
                Console.WriteLine($"Error creating user {email}: {error.Description}");
            }
        }
    }
}


