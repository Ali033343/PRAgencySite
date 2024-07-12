using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PRAgencySite.Models;

namespace PRAgencySite.Data
{
    public class PRAgencyContext : IdentityDbContext<ApplicationUser>
    {
        public PRAgencyContext(DbContextOptions<PRAgencyContext> options) : base(options) { }

        public DbSet<Influencer> Influencers { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Campaign> Campaigns { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<Campaign>()
                .HasOne(c => c.Influencer)
                .WithMany(i => i.Campaigns)
                .HasForeignKey(c => c.InfluencerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Campaign>()
                .HasOne(c => c.Brand)
                .WithMany(b => b.Campaigns)
                .HasForeignKey(c => c.BrandId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

    //public class ApplicationUser : IdentityUser
    //{
    //    public string UserType { get; set; } // Influencer, Brand, Admin
       
    //}
}
