using System.ComponentModel.DataAnnotations;

namespace PortfolioCMS.Models
{
    public class Testimonial : BaseEntity
    {
        [Required]
        [StringLength(2000)]
        public string Content { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string ClientName { get; set; } = string.Empty;

        [StringLength(100)]
        public string? ClientTitle { get; set; } = string.Empty;

        [StringLength(100)]
        public string? ClientCompany { get; set; } = string.Empty;

        [StringLength(255)]
        public string? ClientImageUrl { get; set; }

        [Range(1, 5)]
        public int? Rating { get; set; }

        public bool IsApproved { get; set; } = false;
        public bool IsFeatured { get; set; } = false;
        public int DisplayOrder { get; set; }

        [Required]
        public string UserId { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;
    }
}