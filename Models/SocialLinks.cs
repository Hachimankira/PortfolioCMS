using System.ComponentModel.DataAnnotations;

namespace PortfolioCMS.Models
{
    public class SocialLinks : BaseEntity
    {
        [Required]
        [StringLength(50)]
        public string Platform { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string Url { get; set; } = string.Empty;

        [StringLength(255)]
        public string? IconUrl { get; set; } = string.Empty;

        public int DisplayOrder { get; set; }

        [Required]
        public string UserId { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;
    }
}