using System.ComponentModel.DataAnnotations;

namespace PortfolioCMS.Models
{
    public class Experience: BaseEntity
    {
        [Required]
        [StringLength(150)]
        public string Company { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Position { get; set; } = string.Empty;

        [StringLength(100)]
        public string? Location { get; set; }

        [StringLength(50)]
        public string? EmploymentType { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }
        public bool IsCurrent { get; set; } = false;

        [StringLength(2000)]
        public string? Description { get; set; }

        [StringLength(255)]
        public string? CompanyLogoUrl { get; set; }

        public int DisplayOrder { get; set; }

        [Required]
        public string UserId { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;
    }
}