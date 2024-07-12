using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PRAgencySite.Models
{
    public class Brand
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public ApplicationUser User { get; set; }
        public string Name { get; set; }
        public string LogoUrl { get; set; }
        public string Description { get; set; }
        public string Niche { get; set; }
        public ICollection<Campaign> Campaigns { get; set; }
    }
}
