using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace PortfolioCMS.DTOs.SocialPlatform
{
    public class SocialPlatformResponseDto
    {
        [SwaggerSchema(Description = "Unique identifier of the platform")]
        public int Id { get; set; }

        [SwaggerSchema(Description = "Name of the platform")]
        public string Name { get; set; } = string.Empty;

        [SwaggerSchema(Description = "SVG Icon string")]
        public string SvgIcon { get; set; } = string.Empty;
    }

    public class CreateSocialPlatformDto
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string SvgIcon { get; set; } = string.Empty;
    }

    public class UpdateSocialPlatformDto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string SvgIcon { get; set; } = string.Empty;
    }
}
