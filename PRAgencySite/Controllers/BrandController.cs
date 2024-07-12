// Controllers/BrandController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PRAgencySite.Data;
using PRAgencySite.Models;
using PRAgencySite.Services;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PRAgencySite.Controllers
{
    public class BrandController : Controller
    {
        private readonly PRAgencyContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly TwilioService _twilioService;

        public BrandController(PRAgencyContext context, UserManager<ApplicationUser> userManager, TwilioService twilioService)
        {
            _context = context;
            _userManager = userManager;
            _twilioService = twilioService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var brand = await _context.Brands
                .Include(b => b.Campaigns)
                .ThenInclude(c=>c.Influencer)
                .FirstOrDefaultAsync(b => b.UserId == userId);

            if (brand == null)
            {
                return NotFound();
            }

            return View(brand);
        }

        public async Task<IActionResult> StartCampaign()
        {
            var influencers = await _context.Influencers.ToListAsync();
            var influencerSelectList = influencers.Select(i => new SelectListItem
            {
                Value = i.UserId,
                Text = $"{i.Name} ({i.InstagramHandle})"
            }).ToList();

            var viewModel = new StartCampaignViewModel
            {
                Influencers = influencerSelectList
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> StartCampaign(StartCampaignViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var brand = await _context.Brands.FirstOrDefaultAsync(b => b.UserId == userId);

                if (brand == null)
                {
                    return NotFound("Brand not found");
                }

                var campaign = new Campaign
                {
                    ReelsCount = model.ReelsCount,
                    PostsCount = model.PostsCount,
                    StoriesCount = model.StoriesCount,
                    Budget = model.Budget,
                    Status = "Submitted to Admin",
                    BrandId = brand.Id // use the BrandId from the brand object
                };

                if (!string.IsNullOrEmpty(model.SelectedInfluencerId))
                {
                    campaign.InfluencerId = int.Parse(model.SelectedInfluencerId);
                }

                _context.Campaigns.Add(campaign);
                await _context.SaveChangesAsync();

                // Notify admin via WhatsApp
                var adminPhoneNumber = "+923234301288"; // Replace with your admin's WhatsApp number
                var message = $"New campaign started by {brand.Name}. Reels: {model.ReelsCount}, Posts: {model.PostsCount}, Stories: {model.StoriesCount}, Budget: {model.Budget}" + $"View details at: {Url.Action("Login", "Account", null, Request.Scheme)}";

                 _twilioService.SendWhatsAppMessage(adminPhoneNumber, message);

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }
    }
}

