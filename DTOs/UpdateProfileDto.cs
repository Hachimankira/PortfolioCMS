using System.ComponentModel.DataAnnotations;

namespace PortfolioCMS.Models
{
    public class UpdateProfileDto
    {
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
    }
}