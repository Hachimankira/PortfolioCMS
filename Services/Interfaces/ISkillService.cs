using PortfolioCMS.DTOs.Skill;

namespace PortfolioCMS.Services.Interfaces
{
    public interface ISkillService
    {
        Task<IEnumerable<SkillResponseDto>> GetAllSkillsAsync (string userId);
        Task<SkillResponseDto?> GetSkillByIdAsync(int id , string userId);
        Task<SkillResponseDto> CreateSkillAsync(CreateSkillDto createSkillDto , string userId);
        Task<SkillResponseDto?> UpdateSkillAsync(int id, UpdateSkillDto updateSkillDto , string userId);
        Task<bool> DeleteSkillAsync(int id , string userId);
    }
}