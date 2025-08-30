using PortfolioCMS.DTOs.Experience;

namespace PortfolioCMS.Services.Interfaces
{
    public interface IExperienceService
    {
        Task<IEnumerable<ExperienceResponseDto>> GetAllExperiencesAsync(string userId);
        Task<ExperienceResponseDto?> GetExperienceByIdAsync(int id, string userId);
        Task<ExperienceResponseDto> AddExperienceAsync(CreateExperienceDto experienceDto, string userId);
        Task<ExperienceResponseDto?> UpdateExperienceAsync(int id, UpdateExperienceDto experienceDto, string userId);
        Task<bool> DeleteExperienceAsync(int id, string userId);
    }
}