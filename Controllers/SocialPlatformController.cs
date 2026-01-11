using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using PortfolioCMS.DTOs.SocialPlatform;
using PortfolioCMS.Models.Wrappers;
using PortfolioCMS.Services.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace PortfolioCMS.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("CMSPolicy")]
    [SwaggerTag("Manage social media platforms")]
    public class SocialPlatformController : ControllerBase
    {
        private readonly ISocialPlatformService _service;

        public SocialPlatformController(ISocialPlatformService service)
        {
            _service = service;
        }

        [HttpGet]
        [AllowAnonymous] // Allow public access to list platforms
        [SwaggerOperation(
            Summary = "Get all platforms",
            Description = "Retrieves a list of all supported social media platforms.",
            OperationId = "GetAllSocialPlatforms"
        )]
        [SwaggerResponse(200, "Returns list of platforms", typeof(ApiResponse<IEnumerable<SocialPlatformResponseDto>>))]
        public async Task<IActionResult> GetAll()
        {
            var platforms = await _service.GetAllAsync();
            return Ok(new ApiResponse<IEnumerable<SocialPlatformResponseDto>>(platforms, "Social platforms retrieved successfully"));
        }

        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = "Get platform by ID",
            Description = "Retrieves a specific social platform by its unique identifier.",
            OperationId = "GetSocialPlatformById"
        )]
        [SwaggerResponse(200, "Returns the requested platform", typeof(ApiResponse<SocialPlatformResponseDto>))]
        [SwaggerResponse(404, "Platform not found", typeof(ApiResponse<string>))]
        public async Task<IActionResult> GetById(int id)
        {
            var platform = await _service.GetByIdAsync(id);
            if (platform == null) return NotFound(new ApiResponse<string>("Social platform not found"));
            return Ok(new ApiResponse<SocialPlatformResponseDto>(platform, "Social platform retrieved successfully"));
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Create a platform",
            Description = "Adds a new social media platform to the system.",
            OperationId = "CreateSocialPlatform"
        )]
        [SwaggerResponse(201, "Platform created successfully", typeof(ApiResponse<SocialPlatformResponseDto>))]
        [SwaggerResponse(400, "Invalid input", typeof(ApiResponse<string>))]
        [SwaggerResponse(401, "User is not authorized", typeof(ApiResponse<string>))]
        public async Task<IActionResult> Create([FromBody] CreateSocialPlatformDto dto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                var response = new ApiResponse<string>("Validation failed");
                response.Errors = errors;
                return BadRequest(response);
            }

            var platform = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = platform.Id }, new ApiResponse<SocialPlatformResponseDto>(platform, "Social platform created successfully"));
        }

        [HttpPut("{id}")]
        [SwaggerOperation(
            Summary = "Update a platform",
            Description = "Updates an existing social media platform.",
            OperationId = "UpdateSocialPlatform"
        )]
        [SwaggerResponse(200, "Platform updated successfully", typeof(ApiResponse<SocialPlatformResponseDto>))]
        [SwaggerResponse(404, "Platform not found", typeof(ApiResponse<string>))]
        [SwaggerResponse(400, "Invalid input", typeof(ApiResponse<string>))]
        [SwaggerResponse(401, "User is not authorized", typeof(ApiResponse<string>))]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateSocialPlatformDto dto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                var response = new ApiResponse<string>("Validation failed");
                response.Errors = errors;
                return BadRequest(response);
            }

            var updatedPlatform = await _service.UpdateAsync(id, dto);
            if (updatedPlatform == null) return NotFound(new ApiResponse<string>("Social platform not found"));

            return Ok(new ApiResponse<SocialPlatformResponseDto>(updatedPlatform, "Social platform updated successfully"));
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(
            Summary = "Delete a platform",
            Description = "Permanently removes a social media platform.",
            OperationId = "DeleteSocialPlatform"
        )]
        [SwaggerResponse(200, "Platform deleted successfully", typeof(ApiResponse<bool>))]
        [SwaggerResponse(404, "Platform not found", typeof(ApiResponse<string>))]
        [SwaggerResponse(401, "User is not authorized", typeof(ApiResponse<string>))]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.DeleteAsync(id);
            if (!success) return NotFound(new ApiResponse<string>("Social platform not found"));
            return Ok(new ApiResponse<bool>(true, "Social platform deleted successfully"));
        }
    }
}
