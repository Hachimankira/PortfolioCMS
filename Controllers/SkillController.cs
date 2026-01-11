using System.Security.Claims;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using PortfolioCMS.DTOs.Skill;
using PortfolioCMS.DTOs;
using PortfolioCMS.Models.Wrappers;
using PortfolioCMS.Services.Interfaces;

namespace PortfolioCMS.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("CMSPolicy")]
    [SwaggerTag("Manage technical skills")]
    public class SkillController : ControllerBase
    {
        private readonly ISkillService _skillService;

        public SkillController(ISkillService skillService)
        {
            _skillService = skillService;
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Get all skills",
            Description = "Retrieves a paginated list of skills for the authenticated user.",
            OperationId = "GetSkills"
        )]
        [SwaggerResponse(200, "Returns a paginated list of skills", typeof(PagedResponse<IEnumerable<SkillResponseDto>>))]
        [SwaggerResponse(401, "User is not authorized", typeof(ApiResponse<string>))]
        public async Task<IActionResult> GetAll([FromQuery] PaginationFilter filter)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized(new ApiResponse<string>("User not authorized"));
            var result = await _skillService.GetAllSkillsAsync(userId, filter);
            return Ok(new PagedResponse<IEnumerable<SkillResponseDto>>(result.Items, filter.PageNumber, filter.PageSize, result.TotalCount));
        }

        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = "Get skill by ID",
            Description = "Retrieves a specific skill by its unique identifier.",
            OperationId = "GetSkillById"
        )]
        [SwaggerResponse(200, "Returns the requested skill", typeof(ApiResponse<SkillResponseDto>))]
        [SwaggerResponse(404, "Skill not found", typeof(ApiResponse<string>))]
        [SwaggerResponse(401, "User is not authorized", typeof(ApiResponse<string>))]
        public async Task<IActionResult> GetById(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized(new ApiResponse<string>("User not authorized"));

            var skill = await _skillService.GetSkillByIdAsync(id, userId);
            if (skill == null) return NotFound(new ApiResponse<string>("Skill not found"));

            return Ok(new ApiResponse<SkillResponseDto>(skill, "Skill retrieved successfully"));
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Create a new skill",
            Description = "Adds a new technical skill to the user's portfolio.",
            OperationId = "CreateSkill"
        )]
        [SwaggerResponse(201, "Skill created successfully", typeof(ApiResponse<SkillResponseDto>))]
        [SwaggerResponse(400, "Invalid input or validation failed", typeof(ApiResponse<string>))]
        [SwaggerResponse(401, "User is not authorized", typeof(ApiResponse<string>))]
        public async Task<IActionResult> CreateSkill([FromBody] CreateSkillDto dto)
        {
            if (!ModelState.IsValid) 
            {
                 var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                 var response = new ApiResponse<string>("Validation failed");
                 response.Errors = errors;
                 return BadRequest(response);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized(new ApiResponse<string>("User not authorized"));

            var skill = await _skillService.CreateSkillAsync(dto, userId);
            return CreatedAtAction(nameof(GetById), new { id = skill.Id }, new ApiResponse<SkillResponseDto>(skill, "Skill created successfully"));
        }

        [HttpPut("{id}")]
        [SwaggerOperation(
            Summary = "Update a skill",
            Description = "Updates an existing skill with new details.",
            OperationId = "UpdateSkill"
        )]
        [SwaggerResponse(200, "Skill updated successfully", typeof(ApiResponse<SkillResponseDto>))]
        [SwaggerResponse(404, "Skill not found", typeof(ApiResponse<string>))]
        [SwaggerResponse(400, "Invalid input or validation failed", typeof(ApiResponse<string>))]
        [SwaggerResponse(401, "User is not authorized", typeof(ApiResponse<string>))]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateSkillDto dto)
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

            var updatedSkill = await _skillService.UpdateSkillAsync(id, dto, userId);
            if (updatedSkill == null) return NotFound(new ApiResponse<string>("Skill not found"));

            return Ok(new ApiResponse<SkillResponseDto>(updatedSkill, "Skill updated successfully"));
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(
            Summary = "Delete a skill",
            Description = "Permanently removes a skill from the user's portfolio.",
            OperationId = "DeleteSkill"
        )]
        [SwaggerResponse(200, "Skill deleted successfully", typeof(ApiResponse<bool>))]
        [SwaggerResponse(404, "Skill not found", typeof(ApiResponse<string>))]
        [SwaggerResponse(401, "User is not authorized", typeof(ApiResponse<string>))]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized(new ApiResponse<string>("User not authorized"));

            var deleted = await _skillService.DeleteSkillAsync(id, userId);
            if (!deleted) return NotFound(new ApiResponse<string>("Skill not found"));

            return Ok(new ApiResponse<bool>(true, "Skill deleted successfully"));
        }
    }
}
