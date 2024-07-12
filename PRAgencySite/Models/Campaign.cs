using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using Microsoft.AspNetCore.Identity;

namespace PRAgencySite.Models
{
    public class Campaign
    {
        public int Id { get; set; }
        public int  BrandId { get; set; }
        public Brand Brand { get; set; }
        public int? InfluencerId { get; set; }
        public Influencer Influencer { get; set; }
        public int ReelsCount { get; set; }
        public int PostsCount { get; set; }
        public int StoriesCount { get; set; }
        public decimal Budget { get; set; }
        public string Status { get; set; }
        public decimal? BargainPrice { get; set; }
        public string Remarks { get; set; }
        public string RejectionReason { get; set; }
    }




}
