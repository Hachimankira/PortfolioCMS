using Swashbuckle.AspNetCore.Annotations;

namespace PortfolioCMS.DTOs.Skill
{
    public class SkillResponseDto
    {
        [SwaggerSchema(Description = "Unique identifier of the skill")]
        public int Id { get; set; }

        [SwaggerSchema(Description = "Name of the skill (e.g., C#, React, Azure)")]
        public string Name { get; set; } = string.Empty;

        [SwaggerSchema(Description = "Category of the skill (e.g., Frontend, Backend, DevOps)")]
        public string? Category { get; set; }

        [SwaggerSchema(Description = "Proficiency level (e.g., Beginner, Intermediate, Expert)")]
        public string? Level { get; set; }

        [SwaggerSchema(Description = "Display order for the skill")]
        public int DisplayOrder { get; set; }
    }
}