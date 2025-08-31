namespace PortfolioCMS.DTOs.Skill
{
    public class SkillResponseDto
    {
         public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Category { get; set; }
        public string? Level { get; set; }

        public int DisplayOrder { get; set; }
    }
}