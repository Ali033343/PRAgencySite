using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PRAgencySite.Models;

namespace PRAgencySite.Data
{
    public class PRAgencyContext : IdentityDbContext<IdentityUser>
    {
        public PRAgencyContext(DbContextOptions<PRAgencyContext> options) : base(options) { }
        public DbSet<Influencer> Influencers { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Campaign> Campaigns { get; set; }


    }

    public class ApplicationUser : IdentityUser
    {
        public string UserType { get; set; } // Influencer, Brand, Admin
    }
   

}
