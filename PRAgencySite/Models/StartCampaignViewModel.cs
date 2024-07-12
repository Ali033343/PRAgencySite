using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PRAgencySite.Models
{
    public class StartCampaignViewModel
    {
        public int ReelsCount { get; set; }
        public int PostsCount { get; set; }
        public int StoriesCount { get; set; }
        public decimal Budget { get; set; }
        public string SelectedInfluencerId { get; set; }
        public List<SelectListItem> Influencers { get; set; }
    }


}
