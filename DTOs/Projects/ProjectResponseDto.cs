using PortfolioCMS.Models;

namespace PortfolioCMS.DTOs.Projects
{
    public class ProjectResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? FeaturedImageUrl { get; set; }
        public string? Technologies { get; set; }
        public string? RepoUrl { get; set; }
        public string? LiveUrl { get; set; }
        public bool IsFeatured { get; set; }
        public ProjectStatus Status { get; set; } = ProjectStatus.Completed;
        public int DisplayOrder { get; set; }
    }
}