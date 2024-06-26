using Microsoft.AspNetCore.Mvc;
using PRAgencySite.Data;
using PRAgencySite.Models;
using System.IO;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace PRAgencySite.Controllers
{
    public class AdminController : Controller
    {
        private readonly PRAgencyContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public AdminController(PRAgencyContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        private bool IsAdmin()
        {
            // For now, we're hardcoding the admin check
            // In a real application, you'd check the user's identity and role
            return true;
        }

        public IActionResult Index()
        {
            if (!IsAdmin())
            {
                return Unauthorized();
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddInfluencer(Influencer model)
        {
            if (ModelState.IsValid)
            {
               

                // Add the influencer to the database
                _context.Influencers.Add(model);
                await _context.SaveChangesAsync();

                // Set success message in TempData
                TempData["SuccessMessage"] = "Influencer added successfully!";
                return RedirectToAction("Index");
            }

            // If there's an error, set error message in TempData
            TempData["ErrorMessage"] = "Failed to add influencer. Please check the form.";
            return View(model);
        }


    }
}

