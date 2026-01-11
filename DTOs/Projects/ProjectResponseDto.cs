using PortfolioCMS.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace PortfolioCMS.DTOs.Projects
{
    public class ProjectResponseDto
    {
        [SwaggerSchema(Description = "Unique identifier of the project")]
        public int Id { get; set; }

        [SwaggerSchema(Description = "Title of the project")]
        public string Title { get; set; } = string.Empty;

        [SwaggerSchema(Description = "Detailed description of the project")]
        public string? Description { get; set; }

        [SwaggerSchema(Description = "URL of the featured image")]
        public string? FeaturedImageUrl { get; set; }

        [SwaggerSchema(Description = "Comma-separated list of technologies used")]
        public string? Technologies { get; set; }

        [SwaggerSchema(Description = "URL to the source code repository")]
        public string? RepoUrl { get; set; }

        [SwaggerSchema(Description = "URL to the live demo of the project")]
        public string? LiveUrl { get; set; }

        [SwaggerSchema(Description = "Indicates if the project is featured on the portfolio")]
        public bool IsFeatured { get; set; }

        [SwaggerSchema(Description = "Current status of the project (e.g., Completed, InProgress)")]
        public ProjectStatus Status { get; set; } = ProjectStatus.Completed;

        [SwaggerSchema(Description = "Display order for the project")]
        public int DisplayOrder { get; set; }
    }
}