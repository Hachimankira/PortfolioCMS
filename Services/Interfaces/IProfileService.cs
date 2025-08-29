using PortfolioCMS.Models;

namespace PortfolioCMS.Services.Interfaces
{
    public interface IProfileService
    {
        Task<ApplicationUser?> GetProfileAsync(string userId);
        Task<bool> UpdateProfileAsync(string userId, UpdateProfileDto dto);
    }
}