using Microsoft.EntityFrameworkCore;
using PortfolioCMS.Data;
using PortfolioCMS.Models;
using PortfolioCMS.Services.Interfaces;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;

namespace PortfolioCMS.Services.Implementation
{
    public class ApiKeyService : IApiKeyService
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ApiKeyService(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<bool> ValidateApiKeyAsync(string apiKey)
        {
            // Check if the API key exists in the database
            var exists = await _context.ApiKeys.AnyAsync(k => k.Key == apiKey && k.IsActive);
            return exists;
        }

        public async Task<string> GetUsernameFromApiKeyAsync(string apiKey)
        {
            var apiKeyEntity = await _context.ApiKeys
                .Include(k => k.User)
                .FirstOrDefaultAsync(k => k.Key == apiKey && k.IsActive);

            return apiKeyEntity?.User?.UserName ?? string.Empty;
        }

        public async Task<string> GenerateApiKeyAsync(string userId)
        {
            // Generate a new API key
            var key = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
            
            // Create or update the API key for the user
            var existingKey = await _context.ApiKeys.FirstOrDefaultAsync(k => k.UserId == userId);
            
            if (existingKey != null)
            {
                existingKey.Key = key;
                existingKey.UpdatedAt = DateTime.UtcNow;
            }
            else
            {
                var newKey = new ApiKey
                {
                    UserId = userId,
                    Key = key,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                _context.ApiKeys.Add(newKey);
            }
            
            await _context.SaveChangesAsync();
            return key;
        }

        public async Task<string> GetApiKeyByUserIdAsync(string userId)
        {
            var apiKey = await _context.ApiKeys
                .FirstOrDefaultAsync(k => k.UserId == userId && k.IsActive);
                
            return apiKey?.Key ?? string.Empty;
        }
    }
}