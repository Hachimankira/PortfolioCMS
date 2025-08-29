using System.ComponentModel.DataAnnotations;

namespace PortfolioCMS.DTOs
{
    public class CreateEducationDto
    {
        [Required]
        [StringLength(150)]
        public string Institution { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Degree { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Description { get; set; }

        [StringLength(100)]
        public string? FieldOfStudy { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public bool IsCurrent { get; set; } = false;

        [StringLength(255)]
        public string? InstitutionLogoUrl { get; set; }

        public int DisplayOrder { get; set; }
    }
}