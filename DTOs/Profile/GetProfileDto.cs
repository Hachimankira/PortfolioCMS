using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace PortfolioCMS.DTOs
{
    public class GetProfileDto
    {
        [SwaggerSchema(Description = "User's unique identifier")]
        public string? Id { get; set; } 

        [StringLength(100)]
        [SwaggerSchema(Description = "Username of the user")]
        public string? UserName { get; set; } 

        [StringLength(100)]
        [SwaggerSchema(Description = "Full name of the user")]
        public string? FullName { get; set; }

        [EmailAddress]
        [StringLength(50)]
        [SwaggerSchema(Description = "Email address of the user")]
        public string? Email { get; set; }

        [Phone]
        [StringLength(15)]
        [SwaggerSchema(Description = "Phone number")]
        public string? PhoneNumber { get; set; }

        [Url]
        [StringLength(255)]
        [SwaggerSchema(Description = "URL of the profile picture")]
        public string? ProfilePictureUrl { get; set; }

        [StringLength(100)]
        [SwaggerSchema(Description = "Professional headline or job title")]
        public string? Headline { get; set; }

        [StringLength(1000)]
        [SwaggerSchema(Description = "Professional summary or bio")]
        public string? Summary { get; set; }

        [StringLength(100)]
        [SwaggerSchema(Description = "Location (City, Country)")]
        public string? Location { get; set; }

        [SwaggerSchema(Description = "Date when the profile was created")]
        public DateTime CreatedAt { get; set; }

        [SwaggerSchema(Description = "Date when the profile was last updated")]
        public DateTime UpdatedAt { get; set; }
    }
}