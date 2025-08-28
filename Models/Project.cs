using System.ComponentModel.DataAnnotations;

namespace PortfolioCMS.Models
{
    public enum ProjectStatus
    {
        InProgress,
        Completed,
        OnHold,
        Cancelled
    }
    public class Project : BaseEntity
    {
        [Required]
        [StringLength(150)]
        public string Title { get; set; } = string.Empty;

        [StringLength(2000)]
        public string? Description { get; set; }

        [StringLength(255)]
        public string? FeaturedImageUrl { get; set; }

        [StringLength(500)]
        public string? Technologies { get; set; }

        [StringLength(255)]
        public string? RepoUrl { get; set; }

        [StringLength(255)]
        public string? LiveUrl { get; set; }

        public bool IsFeatured { get; set; } = false;
        public ProjectStatus Status { get; set; } = ProjectStatus.Completed;
        public int DisplayOrder { get; set; }

        [Required]
        public string UserId { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;
    }
}