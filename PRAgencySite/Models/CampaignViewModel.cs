using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace PRAgencySite.Models
{
    public class CampaignViewModel
    {
        public int Id { get; set; }
        public string BrandId { get; set; }
        public string InfluencerId { get; set; }
        public int NumberOfReels { get; set; }
        public int NumberOfPosts { get; set; }
        public int NumberOfStories { get; set; }
        public decimal Budget { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }

        public IEnumerable<SelectListItem> Influencers { get; set; }
        public IEnumerable<SelectListItem> Brands { get; set; }
    }

}
