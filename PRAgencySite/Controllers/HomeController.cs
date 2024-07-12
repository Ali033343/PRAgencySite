using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;


        public HomeController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, PRAgencyContext context, InstagramService instagramService, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            _instagramService = instagramService;
            _userManager = userManager;
            _signInManager = signInManager;


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
        public IActionResult GetImageBrand(string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl))
            {
                return NotFound();
            }

            var filePath = Path.Combine(_hostingEnvironment.WebRootPath, "images/"+imageUrl.TrimStart('/'));
            var fileExists = System.IO.File.Exists(filePath);
            if (!fileExists)
            {
                return NotFound();
            }

            var image = System.IO.File.OpenRead(filePath);
            return File(image, "image/jpeg");
        }
        [Authorize]
        public async Task<IActionResult> Index()
        {
            if (_signInManager.IsSignedIn(User))
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    // Handle the case where user is not found
                    return NotFound("User not found");
                }

                var roles = await _userManager.GetRolesAsync(user);

                if (roles.Contains("Admin"))
                {
                    return RedirectToAction("ManageCampaigns", "Admin");
                }
                else if (roles.Contains("Influencer"))
                {
                    return RedirectToAction("Index", "Influencer");
                }
                else if (roles.Contains("Brand"))
                {
                    return RedirectToAction("Index", "Brand");
                }
            }

            var influencers = await _context.Influencers.ToListAsync();
            foreach (var influencer in influencers)
            {
                var userProfile = await _instagramService.GetUserProfileAsync(influencer.InstagramHandle);

                if (userProfile != null && userProfile.Data != null)
                {
                    influencer.FollowersCount = userProfile.Data.FollowerCount;
                    influencer.ProfilePictureUrl = userProfile.Data.ProfilePicUrlHd;
                }
            }

            // Order influencers by FollowersCount in descending order
            var orderedInfluencers = influencers.OrderByDescending(i => i.FollowersCount).ToList();

            var brands = await _context.Brands.ToListAsync();

            var viewModel = new HomeIndexViewModel
            {
                Influencers = orderedInfluencers,
                Brands = brands
            };

            return View(viewModel);
        }

      



    }
}
