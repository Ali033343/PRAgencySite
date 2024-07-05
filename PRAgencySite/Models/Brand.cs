using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PRAgencySite.Models
{
    public class Brand
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string ContactEmail { get; set; }
        [Required]
        public string ContactPhone { get; set; }

        public ICollection<Campaign> Campaigns { get; set; }
    }
}
