using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortfolioCMS.Models
{
    public class ApiKey
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [StringLength(64)]
        public string Key { get; set; } = string.Empty;
        
        [Required]
        public string UserId { get; set; } = null!;
        
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; } = null!;
        
        public bool IsActive { get; set; } = true;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}