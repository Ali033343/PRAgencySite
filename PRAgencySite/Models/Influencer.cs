using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PRAgencySite.Models
{
    public class Influencer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string InstagramHandle { get; set; }
        public int FollowersCount { get; set; }
        public string ProfilePictureUrl { get; set; }
        public string Niche {  get; set; }

        [NotMapped]
        public IFormFile ProfilePicture { get; set; }
    }


}
