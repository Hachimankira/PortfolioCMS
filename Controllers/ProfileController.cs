using System.Security.Claims;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortfolioCMS.Services.Interfaces;
using PortfolioCMS.Models;
using Microsoft.AspNetCore.Cors;
using PortfolioCMS.Models.Wrappers; 
using PortfolioCMS.DTOs; 

namespace PortfolioCMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    [EnableCors("CMSPolicy")]
    [SwaggerTag("Manage user profile")]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService _profileService;
        public ProfileController(IProfileService profileService)
        {
            _profileService = profileService;
        }
        // GET: api/profile
        [HttpGet]
        [SwaggerOperation(
            Summary = "Get basic profile",
            Description = "Retrieves the basic profile information for the authenticated user.",
            OperationId = "GetProfile"
        )]
        [SwaggerResponse(200, "Returns the user profile", typeof(ApiResponse<GetProfileDto>))]
        [SwaggerResponse(404, "Profile not found", typeof(ApiResponse<string>))]
        [SwaggerResponse(401, "User is not authorized", typeof(ApiResponse<string>))]
        public async Task<IActionResult> GetProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized(new ApiResponse<string>("User not authorized"));

            var user = await _profileService.GetProfileAsync(userId);
            if (user == null) return NotFound(new ApiResponse<string>("Profile not found"));

            return Ok(new ApiResponse<object>(user, "Profile retrieved successfully"));
        }

        // GET: api/profile/full
        [HttpGet("full")]
        [SwaggerOperation(
            Summary = "Get full profile",
            Description = "Retrieves the full profile including related entities for the authenticated user.",
            OperationId = "GetFullProfile"
        )]
        [SwaggerResponse(200, "Returns the full user profile", typeof(ApiResponse<object>))]
        [SwaggerResponse(404, "Profile not found", typeof(ApiResponse<string>))]
        [SwaggerResponse(401, "User is not authorized", typeof(ApiResponse<string>))]
        public async Task<IActionResult> GetFullProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized(new ApiResponse<string>("User not authorized"));

            var fullProfile = await _profileService.GetFullProfileAsync(userId);
            if (fullProfile == null) return NotFound(new ApiResponse<string>("Profile not found"));

            return Ok(new ApiResponse<object>(fullProfile, "Full profile retrieved successfully"));
        }

        // PUT: api/profile
        [HttpPut]
        [SwaggerOperation(
            Summary = "Update profile",
            Description = "Updates the user's profile information.",
            OperationId = "UpdateProfile"
        )]
        [SwaggerResponse(200, "Profile updated successfully", typeof(ApiResponse<string>))]
        [SwaggerResponse(400, "Update failed or invalid input", typeof(ApiResponse<string>))]
        [SwaggerResponse(401, "User is not authorized", typeof(ApiResponse<string>))]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized(new ApiResponse<string>("User not authorized"));

            var success = await _profileService.UpdateProfileAsync(userId, dto);
            if (!success) return BadRequest(new ApiResponse<string>("Update Failed"));

            return Ok(new ApiResponse<string>("Profile updated successfully"));
        }
    }
}