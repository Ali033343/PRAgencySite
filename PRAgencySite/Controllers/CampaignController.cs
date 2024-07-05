// Controllers/CampaignController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRAgencySite.Data;
using PRAgencySite.Models;
using System.Linq;
using System.Threading.Tasks;

namespace PRAgencySite.Controllers
{
    public class CampaignController : Controller
    {
        private readonly PRAgencyContext _context;

        public CampaignController(PRAgencyContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var campaigns = _context.Campaigns.Include(c => c.Brand).ToList();
            return View(campaigns);
        }

        public IActionResult Create()
        {
            ViewData["Brands"] = _context.Brands.ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Campaign model)
        {
            if (ModelState.IsValid)
            {
                _context.Campaigns.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["Brands"] = _context.Brands.ToList();
            return View(model);
        }
    }
}
