using System.ComponentModel.DataAnnotations;

namespace PortfolioCMS.DTOs
{
    public class EducationResponseDto
    {
        public int Id { get; set; }

        [StringLength(150)]
        public string Institution { get; set; } = string.Empty;

        [StringLength(100)]
        public string Degree { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Description { get; set; }

        [StringLength(100)]
        public string? FieldOfStudy { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public bool IsCurrent { get; set; }

        [StringLength(255)]
        public string? InstitutionLogoUrl { get; set; }

        public int DisplayOrder { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}