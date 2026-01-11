using PortfolioCMS.DTOs;
using PortfolioCMS.Models.Wrappers;

namespace PortfolioCMS.Services.Interfaces
{
    public interface IEducationService
    {
        Task <PagedResult<EducationResponseDto>> GetAllAsync(string userId, PaginationFilter filter);
        Task <EducationResponseDto?> GetByIdAsync(int id, string userId);
        Task <EducationResponseDto> CreateAsync(CreateEducationDto dto, string userId);
        Task <EducationResponseDto?> UpdateAsync(int id, UpdateEducationDto dto, string userId);
        Task <bool> DeleteAsync(int id, string userId);
    }
}