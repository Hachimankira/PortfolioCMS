using System.ComponentModel.DataAnnotations;

namespace PortfolioCMS.Models
{
    public class Language : BaseEntity
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string Proficiency { get; set; } = "Basic"; // Basic, Intermediate, Fluent, Native

        [Required]
        public string UserId { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;
    }
}