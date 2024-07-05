using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace PRAgencySite.Models
{
    public class Campaign
    {
        public int Id { get; set; }
        [Required]
        public string CampaignName { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int NumberOfInfluencers { get; set; }
        [Required]
        public decimal Budget { get; set; }
        [Required]
        public string PostType { get; set; } // e.g., Post, Reel, Story
        [Required]
        public string Niche { get; set; } // e.g., Fashion, Food, Sports
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }

        [ForeignKey("Brand")]
        public int BrandId { get; set; }
        public Brand Brand { get; set; }
    }
}
