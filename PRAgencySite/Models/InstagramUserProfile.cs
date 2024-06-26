using Newtonsoft.Json;

namespace PRAgencySite.Models
{
    public class InstagramUserProfile
    {
        [JsonProperty("data")]
        public Data Data { get; set; }
    }

    public class Data
    {
        [JsonProperty("follower_count")]
        public int FollowerCount { get; set; }

        [JsonProperty("profile_pic_url_hd")]
        public string ProfilePicUrlHd { get; set; }
    }
}
