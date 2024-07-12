using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PRAgencySite.Models
{
   
    public class Influencer
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public string Name { get; set; }
        public string InstagramHandle { get; set; }
        public string Niche { get; set; }
        public int? FollowersCount { get; set; }
        public string ProfilePictureUrl { get; set; }
        public ICollection<Campaign> Campaigns { get; set; }
    }

}
