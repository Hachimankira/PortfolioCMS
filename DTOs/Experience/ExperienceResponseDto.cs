using PortfolioCMS.Models;

namespace PortfolioCMS.DTOs.Experience
{
    public class ExperienceResponseDto
    {
        public int Id { get; set; }
        public string Company { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public EmploymentType EmploymentType { get; set; } = EmploymentType.FullTime;
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsCurrent { get; set; }
        public string Description { get; set; } = string.Empty;
        public string CompanyLogoUrl { get; set; } = string.Empty;
        public int DisplayOrder { get; set; }
    }
}