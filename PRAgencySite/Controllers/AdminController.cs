using Microsoft.AspNetCore.Mvc;
using PRAgencySite.Data;
using PRAgencySite.Models;
using System.IO;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using PRAgencySite.Services;

namespace PRAgencySite.Controllers
{
    public class AdminController : Controller
    {
        private readonly PRAgencyContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly TwilioService _twilioService;
        public AdminController(PRAgencyContext context, UserManager<ApplicationUser> userManager, TwilioService twilioService)
        {
            _context = context;
            _userManager = userManager;
            _twilioService = twilioService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult ManageCampaigns()
        {
            var campaigns = _context.Campaigns.Include(c => c.Brand).Include(c => c.Influencer).ToList();
            var influencers = _context.Influencers.Include(i => i.User).ToList();

            var viewModel = new ManageCampaignsViewModel
            {
                Campaigns = campaigns,
                Influencers = influencers
            };

            return View(viewModel); // Pass the viewModel instead of campaigns
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AssignInfluencer(int campaignId, int ?influencerId)
        {
            var campaign = await _context.Campaigns.FindAsync(campaignId);

            if (campaign == null)
            {
                return NotFound("Campaign not found");
            }

            campaign.InfluencerId = influencerId;
            campaign.Status = influencerId.HasValue ? "Assigned" : "Unassigned";
            await _context.SaveChangesAsync();
            var infId = await _context.Influencers.FindAsync(influencerId);
            var influencer = await _userManager.FindByIdAsync(infId.UserId.ToString());
            if (influencer != null)
            {
                var message = $"You have been assigned a new campaign. View details at: {Url.Action("Index", "Influencer", null, Request.Scheme)}";
                _twilioService.SendWhatsAppMessage(influencer.WhatsAppNumber, message);
            }
            return RedirectToAction("ManageCampaigns");
        }


        [HttpPost]
        public async Task<IActionResult> SubmitBargainToBrand(int campaignId)
        {
            var campaign = await _context.Campaigns.FindAsync(campaignId);

            if (campaign == null)
            {
                return NotFound();
            }

            campaign.Status = "Reviewed Bargain by Admin";

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(ManageCampaigns));
        }

        [HttpPost]
        public async Task<IActionResult> ReassignInfluencer(int campaignId, int newInfluencerId)
        {
            var campaign = await _context.Campaigns.FindAsync(campaignId);

            if (campaign == null)
            {
                return NotFound();
            }

            campaign.InfluencerId = newInfluencerId;
            campaign.Status = "Assigned";

            await _context.SaveChangesAsync();
            var infId = await _context.Influencers.FindAsync(newInfluencerId);
            var influencer = await _userManager.FindByIdAsync(infId.UserId.ToString());
            if (influencer != null)
            {
                var message = $"You have been assigned a new campaign. View details at: {Url.Action("Index", "Influencer", null, Request.Scheme)}";
                _twilioService.SendWhatsAppMessage(influencer.WhatsAppNumber, message);
            }
            return RedirectToAction(nameof(ManageCampaigns));
        }
    }
}



