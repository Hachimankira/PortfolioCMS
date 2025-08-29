using PortfolioCMS.DTOs;

namespace PortfolioCMS.Services.Interfaces
{
    public interface IEducationService
    {
        Task <IEnumerable<EducationResponseDto>> GetAllAsync(string userId);
        Task <EducationResponseDto?> GetByIdAsync(int id, string userId);
        Task <EducationResponseDto> CreateAsync(CreateEducationDto dto, string userId);
        Task <EducationResponseDto?> UpdateAsync(int id, UpdateEducationDto dto, string userId);
        Task <bool> DeleteAsync(int id, string userId);
    }
}