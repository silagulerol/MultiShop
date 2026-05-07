using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MultiShop.IdentityServer.Models;

namespace MultiShop.IdentityServer.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<VendorProfile> VendorProfiles { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<VendorProfile>()
                .HasOne(x => x.User)
                .WithOne()
                .HasForeignKey<VendorProfile>(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}