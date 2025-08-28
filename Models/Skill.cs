using System.ComponentModel.DataAnnotations;

namespace PortfolioCMS.Models
{
    public class Skill : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(50)]
        public string? Category { get; set; }

        [StringLength(50)]
        public string? Level { get; set; } // Beginner, Intermediate, Expert

        public int DisplayOrder { get; set; }

        [Required]
        public string UserId { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;
    }
}