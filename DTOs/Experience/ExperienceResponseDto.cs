using PortfolioCMS.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace PortfolioCMS.DTOs.Experience
{
    public class ExperienceResponseDto
    {
        [SwaggerSchema(Description = "Unique identifier of the experience record")]
        public int Id { get; set; }

        [SwaggerSchema(Description = "Name of the company")]
        public string Company { get; set; } = string.Empty;

        [SwaggerSchema(Description = "Job title or position")]
        public string Position { get; set; } = string.Empty;

        [SwaggerSchema(Description = "Location of the company")]
        public string Location { get; set; } = string.Empty;

        [SwaggerSchema(Description = "Type of employment (e.g., Full-time, Part-time, Contract)")]
        public EmploymentType EmploymentType { get; set; } = EmploymentType.FullTime;

        [SwaggerSchema(Description = "Start date of the employment", Format = "date")]
        public DateTime StartDate { get; set; }

        [SwaggerSchema(Description = "End date of the employment (null if currently employed)", Format = "date")]
        public DateTime? EndDate { get; set; }

        [SwaggerSchema(Description = "Indicates if this is the current job")]
        public bool IsCurrent { get; set; }

        [SwaggerSchema(Description = "Description of responsibilities and achievements")]
        public string Description { get; set; } = string.Empty;

        [SwaggerSchema(Description = "URL of the company logo")]
        public string CompanyLogoUrl { get; set; } = string.Empty;

        [SwaggerSchema(Description = "Display order for the experience record")]
        public int DisplayOrder { get; set; }
    }
}