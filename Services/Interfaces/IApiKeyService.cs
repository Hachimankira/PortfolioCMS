namespace PortfolioCMS.Services.Interfaces
{
    public interface IApiKeyService
    {
        Task<bool> ValidateApiKeyAsync(string apiKey);
        Task<string> GetUsernameFromApiKeyAsync(string apiKey);
        Task<string> GenerateApiKeyAsync(string userId);
        Task<string> GetApiKeyByUserIdAsync(string userId);
    }
}