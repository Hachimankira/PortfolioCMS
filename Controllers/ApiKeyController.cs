using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using PortfolioCMS.Services.Interfaces;

namespace PortfolioCMS.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("CMSPolicy")]
    public class ApiKeyController : ControllerBase
    {
        private readonly IApiKeyService _apiKeyService;

        public ApiKeyController(IApiKeyService apiKeyService)
        {
            _apiKeyService = apiKeyService;
        }

        [HttpGet]
        public async Task<IActionResult> GetApiKey()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();
            
            var apiKey = await _apiKeyService.GetApiKeyByUserIdAsync(userId);
            
            if (string.IsNullOrEmpty(apiKey))
            {
                apiKey = await _apiKeyService.GenerateApiKeyAsync(userId);
            }
            
            return Ok(new { apiKey });
        }

        [HttpPost("regenerate")]
        public async Task<IActionResult> RegenerateApiKey()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();
            
            var apiKey = await _apiKeyService.GenerateApiKeyAsync(userId);
            return Ok(new { apiKey });
        }
    }
}