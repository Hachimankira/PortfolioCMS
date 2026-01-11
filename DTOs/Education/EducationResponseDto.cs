using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace PortfolioCMS.DTOs
{
    public class EducationResponseDto
    {
        [SwaggerSchema(Description = "Unique identifier of the education record")]
        public int Id { get; set; }

        [StringLength(150)]
        [SwaggerSchema(Description = "Name of the educational institution")]
        public string Institution { get; set; } = string.Empty;

        [StringLength(100)]
        [SwaggerSchema(Description = "Degree obtained")]
        public string Degree { get; set; } = string.Empty;

        [StringLength(1000)]
        [SwaggerSchema(Description = "Additional details about the education")]
        public string? Description { get; set; }

        [StringLength(100)]
        [SwaggerSchema(Description = "Field of study")]
        public string? FieldOfStudy { get; set; }

        [SwaggerSchema(Description = "Start date of the education", Format = "date")]
        public DateTime StartDate { get; set; }

        [SwaggerSchema(Description = "End date of the education (null if current)", Format = "date")]
        public DateTime? EndDate { get; set; }

        [SwaggerSchema(Description = "Indicates if this is the current education")]
        public bool IsCurrent { get; set; }

        [StringLength(255)]
        [SwaggerSchema(Description = "URL of the institution logo")]
        public string? InstitutionLogoUrl { get; set; }

        [SwaggerSchema(Description = "Display order for the education record")]
        public int DisplayOrder { get; set; }

        [SwaggerSchema(Description = "Date when the record was created")]
        public DateTime CreatedAt { get; set; }

        [SwaggerSchema(Description = "Date when the record was last updated")]
        public DateTime UpdatedAt { get; set; }
    }
}