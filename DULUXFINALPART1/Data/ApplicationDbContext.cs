using DULUXFINALPART1.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DULUXFINALPART1.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser> // Using ApplicationUser here
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Custom configuration if needed, for now, it's empty as seeding is moved to Program.cs
        }

        public DbSet<GuardsPage> Guards { get; set; }
        public DbSet<Scan_Image> Scan_Images { get; set; }
        public DbSet<Return> Returns { get; set; }
    }
}
