using Microsoft.EntityFrameworkCore;
using PRAgencySite.Models;

namespace PRAgencySite.Data
{
    public class PRAgencyContext : DbContext
    {
        public PRAgencyContext(DbContextOptions<PRAgencyContext> options) : base(options) { }
        public DbSet<Influencer> Influencers { get; set; }
    }

}
