using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRAgencySite.Data;
using PRAgencySite.Models;
using PRAgencySite.Services;
using System.Threading.Tasks;

namespace PRAgencySite.Controllers
{
    public class InfluencerController : Controller
    {
        private readonly PRAgencyContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly TwilioService _twilioService;

        public InfluencerController(PRAgencyContext context, UserManager<ApplicationUser> userManager, TwilioService twilioService)
        {
            _context = context;
            _userManager = userManager;
            _twilioService = twilioService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var influencer = await _context.Influencers
                .Include(i => i.Campaigns)
                .ThenInclude(c => c.Brand)
                .FirstOrDefaultAsync(i => i.UserId == userId);

           

            if (influencer == null)
            {
                return NotFound();
            }

            return View(influencer);
        }

        [HttpPost]
        public async Task<IActionResult> AcceptCampaign(int campaignId)
        {
            var campaign = await _context.Campaigns.FindAsync(campaignId);
            var userId = _userManager.GetUserId(User);
            var influencer = await _context.Influencers
                .Include(i => i.Campaigns)
                .ThenInclude(c => c.Brand)
                .FirstOrDefaultAsync(i => i.UserId == userId);
            if (campaign == null)
            {
                return NotFound();
            }

            campaign.Status = "Accepted by Influencer";
            await _context.SaveChangesAsync();
            var adminPhoneNumber = "+923234301288"; // Replace with your admin's WhatsApp number
            var message = $"campaign accepted by {influencer.Name}. Reels: {campaign.ReelsCount}, Posts: {campaign.PostsCount}, Stories: {campaign.StoriesCount}, Budget: {campaign.Budget}" + $"View details at: {Url.Action("ManageCampaigns", "Admin", null, Request.Scheme)}";

            _twilioService.SendWhatsAppMessage(adminPhoneNumber, message);

            return RedirectToAction(nameof(Index));

           
        }

        [HttpPost]
        public async Task<IActionResult> RejectCampaign(int campaignId, string reason)
        {
            var campaign = await _context.Campaigns.FindAsync(campaignId);
            var userId = _userManager.GetUserId(User);
            var influencer = await _context.Influencers
                .Include(i => i.Campaigns)
                .ThenInclude(c => c.Brand)
                .FirstOrDefaultAsync(i => i.UserId == userId);
            if (campaign == null)
            {
                return NotFound();
            }

            campaign.Status = "Rejected by Influencer";
            campaign.Remarks = "";
            campaign.RejectionReason = reason;
            await _context.SaveChangesAsync();
            var adminPhoneNumber = "+923234301288"; // Replace with your admin's WhatsApp number
            var message = $"Campaign rejected by {influencer.Name}. Reels: {campaign.ReelsCount}, Posts: {campaign.PostsCount}, Stories: {campaign.StoriesCount}, Budget: {campaign.Budget},Reason:{campaign.RejectionReason}" + $"View details at: {Url.Action("ManageCampaigns", "Admin", null, Request.Scheme)}";

            _twilioService.SendWhatsAppMessage(adminPhoneNumber, message);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> BargainCampaign(int campaignId, decimal newPrice)
        {
            var campaign = await _context.Campaigns.FindAsync(campaignId);
            var userId = _userManager.GetUserId(User);
            var influencer = await _context.Influencers
                .Include(i => i.Campaigns)
                .ThenInclude(c => c.Brand)
                .FirstOrDefaultAsync(i => i.UserId == userId);
            if (campaign == null)
            {
                return NotFound();
            }

            campaign.Status = "Bargain Requested by Influencer";
            campaign.BargainPrice = newPrice;
            await _context.SaveChangesAsync();
            var adminPhoneNumber = "+923234301288"; // Replace with your admin's WhatsApp number
            var message = $"Campaign bargained request by {influencer.Name}. Reels: {campaign.ReelsCount}, Posts: {campaign.PostsCount}, Stories: {campaign.StoriesCount}, Budget: {campaign.Budget},Bargain Rate:{campaign.BargainPrice}" + $"View details at: {Url.Action("ManageCampaigns", "Admin", null, Request.Scheme)}";

            _twilioService.SendWhatsAppMessage(adminPhoneNumber, message);
            return RedirectToAction(nameof(Index));
        }
    }
}