using PortfolioCMS.DTOs;
using PortfolioCMS.Models;

namespace PortfolioCMS.Services.Interfaces
{
    public interface IProfileService
    {
        Task<GetProfileDto?> GetProfileAsync(string userId);
        Task<FullProfileDto?> GetFullProfileAsync(string userId); 
        Task<bool> UpdateProfileAsync(string userId, UpdateProfileDto dto);
    }
}