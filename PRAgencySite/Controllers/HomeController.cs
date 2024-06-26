using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PRAgencySite.Data;
using PRAgencySite.Models;
using PRAgencySite.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PRAgencySite.Controllers
{
    public class HomeController : Controller
    {
        private readonly PRAgencyContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly InstagramService _instagramService;

        public HomeController(PRAgencyContext context, IWebHostEnvironment hostingEnvironment, InstagramService instagramService)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            _instagramService = instagramService;
        }

        // HomeController.cs
        public async Task<IActionResult> GetImage(string imageUrl)
        {
            try
            {
                HttpClient client = new HttpClient();
                var imageBytes = await client.GetByteArrayAsync(imageUrl);
                return File(imageBytes, "image/jpeg"); // Adjust content type based on your image type
            }
            catch (Exception ex)
            {
                // Handle any exceptions here
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        public async Task<IActionResult> Index()
        {
            
            var influencers = _context.Influencers.ToList();
            foreach (var influencer in influencers)
            {
                var userProfile = await _instagramService.GetUserProfileAsync(influencer.InstagramHandle);

                if (userProfile != null && userProfile.Data != null)
                {
                    influencer.FollowersCount = userProfile.Data.FollowerCount;
                    influencer.ProfilePictureUrl = userProfile.Data.ProfilePicUrlHd;
                }
            }
            return View(influencers);
        }
    }


}
