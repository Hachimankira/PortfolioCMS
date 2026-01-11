using PortfolioCMS.DTOs.SocialPlatform;

namespace PortfolioCMS.Services.Interfaces
{
    public interface ISocialPlatformService
    {
        Task<IEnumerable<SocialPlatformResponseDto>> GetAllAsync();
        Task<SocialPlatformResponseDto?> GetByIdAsync(int id);
        Task<SocialPlatformResponseDto> CreateAsync(CreateSocialPlatformDto dto);
        Task<SocialPlatformResponseDto?> UpdateAsync(int id, UpdateSocialPlatformDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
