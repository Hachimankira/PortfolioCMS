using PortfolioCMS.Models.DTOs;

namespace PortfolioCMS.Services.Interfaces
{
    public interface ISocialLinksService
    {
        Task<IEnumerable<LinkResponseDto>> GetAllAsync(string userId);
        Task<LinkResponseDto?> GetByIdAsync(int id, string userId);
        Task<LinkResponseDto> CreateAsync(CreateLinkDto dto, string userId);
        Task<LinkResponseDto?> UpdateAsync(UpdateLinkDto dto, string userId);
        Task<bool> DeleteAsync(int id, string userId);
    }
}