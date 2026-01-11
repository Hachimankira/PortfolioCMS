using PortfolioCMS.DTOs;
using PortfolioCMS.Models;
using PortfolioCMS.Models.Wrappers;

namespace PortfolioCMS.Services.Interfaces
{
    public interface ICertification
    {
        Task<PagedResult<CertificationResponseDto>> GetAllAsync(string userId, PaginationFilter filter);
        Task<CertificationResponseDto?> GetByIdAsync(int id, string userId); // Add userId
        Task<CertificationResponseDto> CreateAsync(CreateCertificationDto dto, string userId);
        Task<CertificationResponseDto?> UpdateAsync(UpdateCertificationDto dto, int id, string userId); // Add userId, make nullable
        Task<bool> DeleteAsync(int id, string userId); // Add userId
    }
}