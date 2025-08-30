using System.ComponentModel.DataAnnotations;
using PortfolioCMS.Models;

namespace PortfolioCMS.DTOs.Experience
{
    public class CreateExperienceDto
    {
        [Required]
        [StringLength(150)]
        public string Company { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Position { get; set; } = string.Empty;

        [StringLength(100)]
        public string Location { get; set; } = string.Empty;

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public bool IsCurrent { get; set; }

        [StringLength(2000)]
        public string Description { get; set; } = string.Empty;

        public EmploymentType EmploymentType { get; set; } = EmploymentType.FullTime;

        [StringLength(255)]
        public string CompanyLogoUrl { get; set; } = string.Empty;

        public int DisplayOrder { get; set; }
    }
}