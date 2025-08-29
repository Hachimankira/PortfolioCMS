using System.ComponentModel.DataAnnotations;

namespace PortfolioCMS.DTOs // Change to PortfolioCMS.DTOs for better organization
{
    public class GetProfileDto
    {
        public string? Id { get; set; } // User ID from Identity

        [StringLength(100)]
        public string? UserName { get; set; } // From IdentityUser

        [StringLength(100)]
        public string? FullName { get; set; }

        [EmailAddress]
        [StringLength(50)]
        public string? Email { get; set; }

        [Phone]
        [StringLength(15)]
        public string? PhoneNumber { get; set; }

        [Url]
        [StringLength(255)]
        public string? ProfilePictureUrl { get; set; }

        [StringLength(100)]
        public string? Headline { get; set; }

        [StringLength(1000)]
        public string? Summary { get; set; }

        [StringLength(100)]
        public string? Location { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}