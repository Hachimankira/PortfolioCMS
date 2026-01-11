using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace PortfolioCMS.Models.DTOs
{
    public class LinkResponseDto
    {
        [SwaggerSchema(Description = "Unique identifier of the social link")]
        public int Id { get; set; }

        [SwaggerSchema(Description = "Name of the platform (e.g., LinkedIn, GitHub)")]
        public string Platform { get; set; } = string.Empty;

        [SwaggerSchema(Description = "URL of the social profile")]
        public string Url { get; set; } = string.Empty;

        [SwaggerSchema(Description = "URL of the platform icon")]
        public string? IconUrl { get; set; } = string.Empty;

        [SwaggerSchema(Description = "Display order for the link")]
        public int DisplayOrder { get; set; }

        [SwaggerSchema(Description = "ID of the user who owns this link")]
        public string UserId { get; set; } = string.Empty;
        // Optionally include User details if needed, but omitted here to avoid navigation
    }

    public class CreateLinkDto
    {
        [Required]
        [StringLength(50)]
        public string Platform { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        [Url]
        public string Url { get; set; } = string.Empty;

        [StringLength(255)]
        [Url]
        public string? IconUrl { get; set; } = string.Empty;

        public int DisplayOrder { get; set; }
        // UserId can be set from the authenticated user's context
    }

    public class UpdateLinkDto
    {
        public int Id { get; set; } // For identifying the link to update

        [StringLength(50)]
        public string Platform { get; set; } = string.Empty;

        [StringLength(500)]
        [Url]
        public string Url { get; set; } = string.Empty;

        [StringLength(255)]
        [Url]
        public string? IconUrl { get; set; } = string.Empty;

        public int DisplayOrder { get; set; }
        // UserId should not be updatable for security; handle via context
    }
}