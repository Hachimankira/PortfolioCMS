using System.ComponentModel.DataAnnotations;

namespace PortfolioCMS.DTOs
{
    public class CreateCertificationDto
    {
        [Required]
        [StringLength(150)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Issuer { get; set; } = string.Empty;

        [StringLength(255)]
        public string? CredentialUrl { get; set; }

        [StringLength(100)]
        public string? CredentialId { get; set; }

        [Required]
        public DateTime DateIssued { get; set; }

        public DateTime? ExpirationDate { get; set; }
        public bool DoesNotExpire { get; set; } = false;

        public int DisplayOrder { get; set; }
    }
}
