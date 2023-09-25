using Sabio.Models.Requests.Locations;
using System;
using System.ComponentModel.DataAnnotations;

namespace Sabio.Models.Requests.Organizations
{
    public class OrganizationAddRequest
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int OrganizationTypeId { get; set; }

        [Required]
        [StringLength(200, ErrorMessage = "Name required", MinimumLength = 2)]
        public string Name { get; set; }

        [StringLength(200, ErrorMessage = "Headline is required", MinimumLength = 2)]
        public string Headline { get; set; }

        [MaxLength(-1, ErrorMessage = "Description required")]
        public string Description { get; set; }

        [StringLength(255, ErrorMessage = "Logo required", MinimumLength = 2)]
        public string Logo { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int LocationId{ get; set; }

        [StringLength(200, ErrorMessage = "Phone required", MinimumLength = 2)]
        public string Phone { get; set; }

        [StringLength(255, ErrorMessage = "SiteUrl required", MinimumLength = 8)]
        public string SiteUrl { get; set; }
    }
}
