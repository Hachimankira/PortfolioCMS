using Microsoft.EntityFrameworkCore;
using PortfolioCMS.Data;
using PortfolioCMS.DTOs.SocialPlatform;
using PortfolioCMS.Models;
using PortfolioCMS.Services.Interfaces;

namespace PortfolioCMS.Services.Implementation
{
    public class SocialPlatformService : ISocialPlatformService
    {
        private readonly AppDbContext _context;

        public SocialPlatformService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SocialPlatformResponseDto>> GetAllAsync()
        {
            return await _context.SocialPlatforms
                .OrderBy(p => p.Name)
                .Select(p => new SocialPlatformResponseDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    SvgIcon = p.SvgIcon
                })
                .ToListAsync();
        }

        public async Task<SocialPlatformResponseDto?> GetByIdAsync(int id)
        {
            var platform = await _context.SocialPlatforms.FindAsync(id);
            if (platform == null) return null;

            return new SocialPlatformResponseDto
            {
                Id = platform.Id,
                Name = platform.Name,
                SvgIcon = platform.SvgIcon
            };
        }

        public async Task<SocialPlatformResponseDto> CreateAsync(CreateSocialPlatformDto dto)
        {
            var platform = new SocialPlatform
            {
                Name = dto.Name,
                SvgIcon = dto.SvgIcon
            };

            _context.SocialPlatforms.Add(platform);
            await _context.SaveChangesAsync();

            return new SocialPlatformResponseDto
            {
                Id = platform.Id,
                Name = platform.Name,
                SvgIcon = platform.SvgIcon
            };
        }

        public async Task<SocialPlatformResponseDto?> UpdateAsync(int id, UpdateSocialPlatformDto dto)
        {
            var platform = await _context.SocialPlatforms.FindAsync(id);
            if (platform == null) return null;

            platform.Name = dto.Name;
            platform.SvgIcon = dto.SvgIcon;
            platform.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return new SocialPlatformResponseDto
            {
                Id = platform.Id,
                Name = platform.Name,
                SvgIcon = platform.SvgIcon
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var platform = await _context.SocialPlatforms.FindAsync(id);
            if (platform == null) return false;

            _context.SocialPlatforms.Remove(platform);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
