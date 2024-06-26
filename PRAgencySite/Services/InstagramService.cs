namespace PRAgencySite.Services
{
    using System;
    using System.Linq.Expressions;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using PRAgencySite.Models;

    public class InstagramService
    {
        private readonly HttpClient _httpClient;

        public InstagramService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<InstagramUserProfile> GetUserProfileAsync(string username)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://instagram-scraper-api2.p.rapidapi.com/v1/info?username_or_id_or_url={username}"),
                Headers =
            {
                { "x-rapidapi-host", "instagram-scraper-api2.p.rapidapi.com" },
                { "x-rapidapi-key", "3d325b2487mshadaaf3092ca808dp11ef4fjsn35e43d606579" },
            },
            };

            using (var response = await _httpClient.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                var userProfile = JsonConvert.DeserializeObject<InstagramUserProfile>(body);
                return userProfile;
            }
        }
    }
}
