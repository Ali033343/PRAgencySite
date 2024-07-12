using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PRAgencySite.Models
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "Role")]
        public string Role { get; set; }
        public List<SelectListItem> RoleList { get; set; }

        // Influencer specific fields
        [Display(Name = "Name")]
        public string InfluencerName { get; set; }

        [Display(Name = "Instagram Handle")]
        public string InstagramHandle { get; set; }

        [Display(Name = "Niche")]
        public string Niche { get; set; }
        [Display(Name = "NicheBrand")]
        public string NicheBrand { get; set; }

        // Brand specific fields
        [Display(Name = "Brand Name")]
        public string BrandName { get; set; }

        [Display(Name = "Image URL")]
        public string ImageUrl { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }
        [Phone]
        [Display(Name = "WhatsApp Number")]
        public string WhatsAppNumber { get; set; }

        public string WhatsAppNumberBrand { get; set; }
        public IFormFile Logo { get; set; }
    }

}
