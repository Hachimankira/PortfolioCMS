using System.ComponentModel.DataAnnotations;

namespace PortfolioCMS.DTOs.Skill
{
    public class UpdateSkillDto
    {
        [StringLength(100)]
        public string? Name { get; set; } = string.Empty;

        [StringLength(50)]
        public string? Category { get; set; }

        [StringLength(50)]
        public string? Level { get; set; }

        public int? DisplayOrder { get; set; }
    }
}