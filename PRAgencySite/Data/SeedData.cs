﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using PRAgencySite.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PRAgencySite.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            string[] roleNames = { "Admin", "Influencer", "Brand" };
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            var superUser = new ApplicationUser
            {
                UserName = "admin@primepragency.com",
                Email = "admin@primepragency.com",
                WhatsAppNumber = "+923334311217" // replace with actual number
            };

            string userPWD = "Admin@123";
            var _user = await userManager.FindByEmailAsync("admin@primepragency.com");

            if (_user == null)
            {
                var createPowerUser = await userManager.CreateAsync(superUser, userPWD);
                if (createPowerUser.Succeeded)
                {
                    await userManager.AddToRoleAsync(superUser, "Admin");
                }
            }
        }
    }
}