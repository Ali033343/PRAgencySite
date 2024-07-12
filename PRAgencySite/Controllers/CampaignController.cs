using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PRAgencySite.Data;
using PRAgencySite.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

[Authorize]
public class CampaignController : Controller
{
    private readonly PRAgencyContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public CampaignController(PRAgencyContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public IActionResult Index()
    {
        var campaigns = _context.Campaigns.Include(c => c.Brand).Include(c => c.Influencer).ToList();
        return View(campaigns);
    }

    public IActionResult Create()
    {
        var viewModel = new CampaignViewModel
        {
            Influencers = _context.Influencers.Select(i => new SelectListItem { Value = i.UserId, Text = i.Name }).ToList(),
            Brands = _context.Brands.Select(b => new SelectListItem { Value = b.UserId, Text = b.Name }).ToList()
        };
        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CampaignViewModel model)
    {
        if (ModelState.IsValid)
        {
            var campaign = new Campaign
            {
                BrandId = int.Parse(model.BrandId),
                InfluencerId = null,
                ReelsCount = model.NumberOfReels,
                PostsCount = model.NumberOfPosts,
                StoriesCount = model.NumberOfStories,
                Budget = model.Budget,
                Status = "Submitted to Admin",
                Remarks = string.Empty
            };

            _context.Campaigns.Add(campaign);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        model.Influencers = _context.Influencers.Select(i => new SelectListItem { Value = i.UserId, Text = i.Name }).ToList();
        model.Brands = _context.Brands.Select(b => new SelectListItem { Value = b.UserId, Text = b.Name }).ToList();

        return View(model);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var campaign = await _context.Campaigns.FindAsync(id);
        if (campaign == null)
        {
            return NotFound();
        }

        var viewModel = new CampaignViewModel
        {
            Id = campaign.Id,
            BrandId = campaign.BrandId.ToString(),
            InfluencerId = campaign.InfluencerId.ToString(),
            NumberOfReels = campaign.ReelsCount,
            NumberOfPosts = campaign.PostsCount,
            NumberOfStories = campaign.StoriesCount,
            Budget = campaign.Budget,
            Status = campaign.Status,
            Remarks = campaign.Remarks,
            Influencers = _context.Influencers.Select(i => new SelectListItem { Value = i.UserId, Text = i.Name }).ToList(),
            Brands = _context.Brands.Select(b => new SelectListItem { Value = b.UserId, Text = b.Name }).ToList()
        };

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(CampaignViewModel model)
    {
        if (ModelState.IsValid)
        {
            var campaign = await _context.Campaigns.FindAsync(model.Id);
            if (campaign == null)
            {
                return NotFound();
            }

            campaign.InfluencerId = int.Parse(model.InfluencerId);
            campaign.ReelsCount = model.NumberOfReels;
            campaign.PostsCount = model.NumberOfPosts;
            campaign.StoriesCount = model.NumberOfStories;
            campaign.Budget = model.Budget;
            campaign.Status = model.Status;
            campaign.Remarks = model.Remarks;

            _context.Campaigns.Update(campaign);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        model.Influencers = _context.Influencers.Select(i => new SelectListItem { Value = i.UserId, Text = i.Name }).ToList();
        model.Brands = _context.Brands.Select(b => new SelectListItem { Value = b.UserId, Text = b.Name }).ToList();

        return View(model);
    }
}

