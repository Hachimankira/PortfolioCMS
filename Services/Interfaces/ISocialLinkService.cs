using PortfolioCMS.DTOs;
using PortfolioCMS.Models.DTOs;
using PortfolioCMS.Models.Wrappers;

namespace PortfolioCMS.Services.Interfaces
{
    public interface ISocialLinksService
    {
        Task<PagedResult<LinkResponseDto>> GetAllAsync(string userId, PaginationFilter filter);
        Task<LinkResponseDto?> GetByIdAsync(int id, string userId);
        Task<LinkResponseDto> CreateAsync(CreateLinkDto dto, string userId);
        Task<LinkResponseDto?> UpdateAsync(UpdateLinkDto dto, string userId);
        Task<bool> DeleteAsync(int id, string userId);
    }
}