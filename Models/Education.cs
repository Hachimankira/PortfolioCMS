using System.ComponentModel.DataAnnotations;

namespace PortfolioCMS.Models
{
    public class Education : BaseEntity
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
        public string? InstitutionLogoUrl { get; set; } = string.Empty;

        public int DisplayOrder { get; set; }

        [Required]
        public string UserId { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;
    }
}