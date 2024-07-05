// Controllers/BrandController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRAgencySite.Data;
using PRAgencySite.Models;
using System.Linq;
using System.Threading.Tasks;

namespace PRAgencySite.Controllers
{
    public class BrandController : Controller
    {
        private readonly PRAgencyContext _context;

        public BrandController(PRAgencyContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var brands = _context.Brands.ToList();
            return View(brands);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Brand model)
        {
            if (ModelState.IsValid)
            {
                _context.Brands.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(model);
        }
    }
}

