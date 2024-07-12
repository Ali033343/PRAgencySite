using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PRAgencySite.Data;
using PRAgencySite.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace PRAgencySite.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly PRAgencyContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public AccountController(UserManager<ApplicationUser> userManager, PRAgencyContext context, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, IWebHostEnvironment hostingEnvironment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Invalid login attempt.");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    WhatsAppNumber = model.WhatsAppNumber ?? model.WhatsAppNumberBrand,
                    UserType = model.Role
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, model.Role);

                    if (model.Role == "Influencer")
                    {
                        var influencer = new Influencer
                        {
                            UserId = user.Id,
                            Name = model.InfluencerName,
                            InstagramHandle = model.InstagramHandle,
                            Niche = model.Niche
                        };
                        _context.Influencers.Add(influencer);
                    }
                    else if (model.Role == "Brand")
                    {
                        string uniqueFileName = null;

                        if (model.Logo != null)
                        {
                            string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");
                            uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Logo.FileName;
                            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                            using (var fileStream = new FileStream(filePath, FileMode.Create))
                            {
                                await model.Logo.CopyToAsync(fileStream);
                            }
                        }

                        var brand = new Brand
                        {
                            UserId = user.Id,
                            Name = model.BrandName,
                            LogoUrl = uniqueFileName,
                            Description = model.Description,
                            Niche = model.NicheBrand
                        };
                        _context.Brands.Add(brand);
                    }

                    await _context.SaveChangesAsync();

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }



        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
