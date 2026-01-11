using System.Security.Claims;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using PortfolioCMS.DTOs.Experience;
using PortfolioCMS.DTOs;
using PortfolioCMS.Models.Wrappers;
using PortfolioCMS.Services.Interfaces;

namespace PortfolioCMS.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("CMSPolicy")]
    [SwaggerTag("Manage professional experience records")]
    public class ExperienceController : ControllerBase
    {
        private readonly IExperienceService _experienceService;
        public ExperienceController(IExperienceService experienceService)
        {
            _experienceService = experienceService;
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Get all experience records",
            Description = "Retrieves a paginated list of professional experiences for the authenticated user.",
            OperationId = "GetExperienceRecords"
        )]
        [SwaggerResponse(200, "Returns a paginated list of experience records", typeof(PagedResponse<IEnumerable<ExperienceResponseDto>>))]
        [SwaggerResponse(401, "User is not authorized", typeof(ApiResponse<string>))]
        public async Task<IActionResult> GetAll([FromQuery] PaginationFilter filter)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized(new ApiResponse<string>("User not authorized"));
            var result = await _experienceService.GetAllExperiencesAsync(userId, filter);
            return Ok(new PagedResponse<IEnumerable<ExperienceResponseDto>>(result.Items, filter.PageNumber, filter.PageSize, result.TotalCount));
        }
        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = "Get experience record by ID",
            Description = "Retrieves a specific experience record by its unique identifier.",
            OperationId = "GetExperienceById"
        )]
        [SwaggerResponse(200, "Returns the requested experience record", typeof(ApiResponse<ExperienceResponseDto>))]
        [SwaggerResponse(404, "Experience record not found", typeof(ApiResponse<string>))]
        [SwaggerResponse(401, "User is not authorized", typeof(ApiResponse<string>))]
        public async Task<IActionResult> GetById(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized(new ApiResponse<string>("User not authorized"));
            var experience = await _experienceService.GetExperienceByIdAsync(id, userId);
            if (experience == null) return NotFound(new ApiResponse<string>("Experience record not found"));
            return Ok(new ApiResponse<ExperienceResponseDto>(experience, "Experience record retrieved successfully"));
        }
        [HttpPost]
        [SwaggerOperation(
            Summary = "Create a new experience record",
            Description = "Adds a new professional experience to the user's portfolio.",
            OperationId = "CreateExperience"
        )]
        [SwaggerResponse(201, "Experience record created successfully", typeof(ApiResponse<ExperienceResponseDto>))]
        [SwaggerResponse(400, "Invalid input or validation failed", typeof(ApiResponse<string>))]
        [SwaggerResponse(401, "User is not authorized", typeof(ApiResponse<string>))]
        public async Task<IActionResult> CreateExperience([FromBody] CreateExperienceDto dto)
        {
            if (!ModelState.IsValid) 
            {
                 var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                 var response = new ApiResponse<string>("Validation failed");
                 response.Errors = errors;
                 return BadRequest(response); // Returns validation errors wrapped
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized(new ApiResponse<string>("User not authorized"));
            var experience = await _experienceService.AddExperienceAsync(dto, userId);
            return CreatedAtAction(nameof(GetById), new { id = experience.Id }, new ApiResponse<ExperienceResponseDto>(experience, "Experience record created successfully"));
        }
        [HttpPut("{id}")]
        [SwaggerOperation(
            Summary = "Update an experience record",
            Description = "Updates an existing experience record with new details.",
            OperationId = "UpdateExperience"
        )]
        [SwaggerResponse(200, "Experience record updated successfully", typeof(ApiResponse<ExperienceResponseDto>))]
        [SwaggerResponse(404, "Experience record not found", typeof(ApiResponse<string>))]
        [SwaggerResponse(400, "Invalid input or validation failed", typeof(ApiResponse<string>))]
        [SwaggerResponse(401, "User is not authorized", typeof(ApiResponse<string>))]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateExperienceDto dto)
        {
            if (!ModelState.IsValid) 
            {
                 var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                 var response = new ApiResponse<string>("Validation failed");
                 response.Errors = errors;
                 return BadRequest(response); // Returns validation errors wrapped
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized(new ApiResponse<string>("User not authorized"));
            var updatedExperience = await _experienceService.UpdateExperienceAsync(id, dto, userId);
            if (updatedExperience == null) return NotFound(new ApiResponse<string>("Experience record not found"));
            return Ok(new ApiResponse<ExperienceResponseDto>(updatedExperience, "Experience record updated successfully"));
        }
        [HttpDelete("{id}")]
        [SwaggerOperation(
            Summary = "Delete an experience record",
            Description = "Permanently removes an experience record from the user's portfolio.",
            OperationId = "DeleteExperience"
        )]
        [SwaggerResponse(200, "Experience record deleted successfully", typeof(ApiResponse<bool>))]
        [SwaggerResponse(404, "Experience record not found", typeof(ApiResponse<string>))]
        [SwaggerResponse(401, "User is not authorized", typeof(ApiResponse<string>))]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized(new ApiResponse<string>("User not authorized"));
            var deleted = await _experienceService.DeleteExperienceAsync(id, userId);
            if (!deleted) return NotFound(new ApiResponse<string>("Experience record not found"));
            return Ok(new ApiResponse<bool>(true, "Experience record deleted successfully"));
        }
    }
}