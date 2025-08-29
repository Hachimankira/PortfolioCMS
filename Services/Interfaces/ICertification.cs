using PortfolioCMS.DTOs;
using PortfolioCMS.Models;

namespace PortfolioCMS.Services.Interfaces
{
    public interface ICertification
    {
        Task<IEnumerable<CertificationResponseDto>> GetAllAsync(string userId);
        Task<CertificationResponseDto?> GetByIdAsync(int id, string userId); // Add userId
        Task<CertificationResponseDto> CreateAsync(CreateCertificationDto dto, string userId);
        Task<CertificationResponseDto?> UpdateAsync(UpdateCertificationDto dto, int id, string userId); // Add userId, make nullable
        Task<bool> DeleteAsync(int id, string userId); // Add userId
    }
}