using System.ComponentModel.DataAnnotations;

namespace PortfolioCMS.Models.DTOs
{
    public class TestimonialResponseDto
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public string ClientName { get; set; } = string.Empty;
        public string? ClientTitle { get; set; } = string.Empty;
        public string? ClientCompany { get; set; } = string.Empty;
        public string? ClientImageUrl { get; set; }
        public int? Rating { get; set; }
        public bool IsApproved { get; set; }
        public bool IsFeatured { get; set; }
        public int DisplayOrder { get; set; }
    }

    public class CreateTestimonialDto
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
        [Url]
        public string? ClientImageUrl { get; set; }

        [Range(1, 5)]
        public int? Rating { get; set; }

        public bool IsApproved { get; set; } = false;
        public bool IsFeatured { get; set; } = false;
        public int DisplayOrder { get; set; }
    }

    public class UpdateTestimonialDto
    {
        public int Id { get; set; } // For identifying the testimonial to update

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
        [Url]
        public string? ClientImageUrl { get; set; }

        [Range(1, 5)]
        public int? Rating { get; set; }

        public bool IsApproved { get; set; }
        public bool IsFeatured { get; set; }
        public int DisplayOrder { get; set; }
    }
}