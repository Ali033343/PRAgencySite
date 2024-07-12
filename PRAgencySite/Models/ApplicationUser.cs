using Microsoft.AspNetCore.Identity;

namespace PRAgencySite.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string UserType { get; set; } // Influencer, Brand, Admin
        public string WhatsAppNumber { get; set; }
    }
}
