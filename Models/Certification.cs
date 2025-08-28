using System.ComponentModel.DataAnnotations;

namespace PortfolioCMS.Models
{
    public class Certification : BaseEntity
    {
        [Required]
        [StringLength(150)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Issuer { get; set; } = string.Empty;

        [StringLength(255)]
        public string? CredentialUrl { get; set; } = string.Empty;

        [StringLength(100)]
        public string? CredentialId { get; set; } = string.Empty;

        [Required]
        public DateTime DateIssued { get; set; }

        public DateTime? ExpirationDate { get; set; }
        public bool DoesNotExpire { get; set; } = false;
        public int DisplayOrder { get; set; }

        [Required]
        public string UserId { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;
    }
}