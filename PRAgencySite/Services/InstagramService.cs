namespace PRAgencySite.Services
{
    using System;
    using System.Linq.Expressions;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Newtonsoft.Json.Linq;

    public class InstagramService
    {
        private readonly string _accessToken;

        public InstagramService(string accessToken)
        {
            _accessToken = accessToken;
        }

        public async Task<(string profilePictureUrl, int followersCount)> GetInstagramData(string instagramHandle)
        {
            try {
                using (var httpClient = new HttpClient())
                {
                    var url = $"https://graph.instagram.com/{instagramHandle}?fields=id,username,media_count,account_type&access_token={_accessToken}";
                    var response = await httpClient.GetStringAsync(url);
                    var json = JObject.Parse(response);

                    var profilePictureUrl = json["profile_picture_url"].ToString();
                    var followersCount = int.Parse(json["followers_count"].ToString());

                    return (profilePictureUrl, followersCount);
                }
            }
            catch (HttpRequestException ex)
            {
                // Handle the exception (log it, return a default value, etc.)
                Console.WriteLine($"HTTP Request Exception: {ex.Message}");
                // You may also check ex.InnerException for more details if available
                return ("profilePictureUrl", 0);
            }
        }
        
    }
}
