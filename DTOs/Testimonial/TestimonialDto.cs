using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace PortfolioCMS.Models.DTOs
{
    public class TestimonialResponseDto
    {
        [SwaggerSchema(Description = "Unique identifier of the testimonial")]
        public int Id { get; set; }

        [SwaggerSchema(Description = "Content/Body of the testimonial")]
        public string Content { get; set; } = string.Empty;

        [SwaggerSchema(Description = "Name of the client who gave the testimonial")]
        public string ClientName { get; set; } = string.Empty;

        [SwaggerSchema(Description = "Title/Position of the client")]
        public string? ClientTitle { get; set; } = string.Empty;

        [SwaggerSchema(Description = "Company of the client")]
        public string? ClientCompany { get; set; } = string.Empty;

        [SwaggerSchema(Description = "URL of the client's photo")]
        public string? ClientImageUrl { get; set; }

        [SwaggerSchema(Description = "Rating given by the client (1-5)")]
        public int? Rating { get; set; }

        [SwaggerSchema(Description = "Indicates if the testimonial is approved for public display")]
        public bool IsApproved { get; set; }

        [SwaggerSchema(Description = "Indicates if the testimonial is featured")]
        public bool IsFeatured { get; set; }

        [SwaggerSchema(Description = "Display order for the testimonial")]
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