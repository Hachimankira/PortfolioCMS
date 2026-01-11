using PortfolioCMS.DTOs;
using PortfolioCMS.DTOs.Experience;
using PortfolioCMS.Models.Wrappers;

namespace PortfolioCMS.Services.Interfaces
{
    public interface IExperienceService
    {
        Task<PagedResult<ExperienceResponseDto>> GetAllExperiencesAsync(string userId, PaginationFilter filter);
        Task<ExperienceResponseDto?> GetExperienceByIdAsync(int id, string userId);
        Task<ExperienceResponseDto> AddExperienceAsync(CreateExperienceDto experienceDto, string userId);
        Task<ExperienceResponseDto?> UpdateExperienceAsync(int id, UpdateExperienceDto experienceDto, string userId);
        Task<bool> DeleteExperienceAsync(int id, string userId);
    }
}