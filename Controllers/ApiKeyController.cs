using System.Security.Claims;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using PortfolioCMS.Models.Wrappers;
using PortfolioCMS.Services.Interfaces;

namespace PortfolioCMS.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("CMSPolicy")]
    [SwaggerTag("Manage API Keys for Public Access")]
    public class ApiKeyController : ControllerBase
    {
        private readonly IApiKeyService _apiKeyService;

        public ApiKeyController(IApiKeyService apiKeyService)
        {
            _apiKeyService = apiKeyService;
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Get API Key",
            Description = "Retrieves the current API Key for the authenticated user."
        )]
        [SwaggerResponse(200, "Returns the API Key", typeof(ApiResponse<object>))]
        [SwaggerResponse(401, "User is not authorized", typeof(ApiResponse<string>))]
        public async Task<IActionResult> GetApiKey()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized(new ApiResponse<string>("User not authorized"));
            
            var apiKey = await _apiKeyService.GetApiKeyByUserIdAsync(userId);
            
            if (string.IsNullOrEmpty(apiKey))
            {
                apiKey = await _apiKeyService.GenerateApiKeyAsync(userId);
            }
            
            return Ok(new ApiResponse<object>(new { apiKey }, "API key retrieved successfully"));
        }

        [HttpPost("regenerate")]
        [SwaggerOperation(
            Summary = "Regenerate API Key",
            Description = "Generates a new API Key for the authenticated user, invalidating the previous one."
        )]
        [SwaggerResponse(200, "Returns the new API Key", typeof(ApiResponse<object>))]
        [SwaggerResponse(401, "User is not authorized", typeof(ApiResponse<string>))]
        public async Task<IActionResult> RegenerateApiKey()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized(new ApiResponse<string>("User not authorized"));
            
            var apiKey = await _apiKeyService.GenerateApiKeyAsync(userId);
            return Ok(new ApiResponse<object>(new { apiKey }, "API key regenerated successfully"));
        }
    }
}