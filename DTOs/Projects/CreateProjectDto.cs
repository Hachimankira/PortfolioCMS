using System.ComponentModel.DataAnnotations;
using PortfolioCMS.Models;

namespace PortfolioCMS.DTOs.Project
{
    public class CreateProjectDto
    {
        [Required]
        [StringLength(150)]
        public string Title { get; set; } = string.Empty;

        [StringLength(2000)]
        public string? Description { get; set; }

        [Url]
        [StringLength(255)]
        public string? FeaturedImageUrl { get; set; }

        [StringLength(500)]
        public string? Technologies { get; set; }

        [Url]
        [StringLength(255)]
        public string? RepoUrl { get; set; }

        [Url]
        [StringLength(255)]
        public string? LiveUrl { get; set; }

        public bool IsFeatured { get; set; }

        public ProjectStatus? Status { get; set; }

        [Required]
        public int DisplayOrder { get; set; }
    }
}