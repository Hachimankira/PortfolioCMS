using System.ComponentModel.DataAnnotations;

namespace PortfolioCMS.DTOs
{
    public class UpdateEducationDto
    {
        [StringLength(150)]
        public string? Institution { get; set; }

        [StringLength(100)]
        public string? Degree { get; set; }

        [StringLength(1000)]
        public string? Description { get; set; }

        [StringLength(100)]
        public string? FieldOfStudy { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public bool? IsCurrent { get; set; }

        [StringLength(255)]
        public string? InstitutionLogoUrl { get; set; }

        public int? DisplayOrder { get; set; }
    }
}