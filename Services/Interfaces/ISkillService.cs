using PortfolioCMS.DTOs;
using PortfolioCMS.DTOs.Skill;
using PortfolioCMS.Models.Wrappers;

namespace PortfolioCMS.Services.Interfaces
{
    public interface ISkillService
    {
        Task<PagedResult<SkillResponseDto>> GetAllSkillsAsync (string userId, PaginationFilter filter);
        Task<SkillResponseDto?> GetSkillByIdAsync(int id , string userId);
        Task<SkillResponseDto> CreateSkillAsync(CreateSkillDto createSkillDto , string userId);
        Task<SkillResponseDto?> UpdateSkillAsync(int id, UpdateSkillDto updateSkillDto , string userId);
        Task<bool> DeleteSkillAsync(int id , string userId);
    }
}