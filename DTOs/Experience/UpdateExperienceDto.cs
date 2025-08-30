using System.ComponentModel.DataAnnotations;
using PortfolioCMS.Models;

namespace PortfolioCMS.DTOs.Experience
{
    public class UpdateExperienceDto
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Company is required.")]
        public string? Company { get; set; } = string.Empty;
        public string? Position { get; set; } = string.Empty;
        public string? Location { get; set; } = string.Empty;
        public EmploymentType? EmploymentType { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? IsCurrent { get; set; }
        public string? Description { get; set; } = string.Empty;
        public string? CompanyLogoUrl { get; set; } = string.Empty;
        public int? DisplayOrder { get; set; }

        public UpdateExperienceDto()
        {
            EmploymentType = Models.EmploymentType.FullTime;
        }
    }
}