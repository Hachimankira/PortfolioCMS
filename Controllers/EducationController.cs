using System.Security.Claims;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using PortfolioCMS.DTOs;
using PortfolioCMS.Models.Wrappers;
using PortfolioCMS.Services.Interfaces;

namespace PortfolioCMS.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("CMSPolicy")]
    [SwaggerTag("Manage education records")]
    public class EducationController : ControllerBase
    {
        private readonly IEducationService _educationService;

        public EducationController(IEducationService educationService)
        {
            _educationService = educationService;
        }
        [HttpGet]
        [SwaggerOperation(
            Summary = "Get all education records",
            Description = "Retrieves a paginated list of education records for the authenticated user.",
            OperationId = "GetEducationRecords"
        )]
        [SwaggerResponse(200, "Returns a paginated list of education records", typeof(PagedResponse<IEnumerable<EducationResponseDto>>))]
        [SwaggerResponse(401, "User is not authorized", typeof(ApiResponse<string>))]
        public async Task<IActionResult> GetAll([FromQuery] PaginationFilter filter)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized(new ApiResponse<string>("User not authorized"));
            var result = await _educationService.GetAllAsync(userId, filter);
            return Ok(new PagedResponse<IEnumerable<EducationResponseDto>>(result.Items, filter.PageNumber, filter.PageSize, result.TotalCount));
        }
        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = "Get education record by ID",
            Description = "Retrieves a specific education record by its unique identifier.",
            OperationId = "GetEducationById"
        )]
        [SwaggerResponse(200, "Returns the requested education record", typeof(ApiResponse<EducationResponseDto>))]
        [SwaggerResponse(404, "Education record not found", typeof(ApiResponse<string>))]
        [SwaggerResponse(401, "User is not authorized", typeof(ApiResponse<string>))]
        public async Task<IActionResult> GetById(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized(new ApiResponse<string>("User not authorized"));
            var education = await _educationService.GetByIdAsync(id, userId);
            if (education == null) return NotFound(new ApiResponse<string>("Education record not found"));
            return Ok(new ApiResponse<EducationResponseDto>(education, "Education record retrieved successfully"));
        }
        [HttpPost]
        [SwaggerOperation(
            Summary = "Create a new education record",
            Description = "Adds a new education record to the user's portfolio.",
            OperationId = "CreateEducation"
        )]
        [SwaggerResponse(201, "Education record created successfully", typeof(ApiResponse<EducationResponseDto>))]
        [SwaggerResponse(400, "Invalid input or validation failed", typeof(ApiResponse<string>))]
        [SwaggerResponse(401, "User is not authorized", typeof(ApiResponse<string>))]
        public async Task<IActionResult> CreateEducation([FromBody] CreateEducationDto dto)
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
            try
            {
                var education = await _educationService.CreateAsync(dto, userId);
                return CreatedAtAction(nameof(GetById), new { id = education.Id }, new ApiResponse<EducationResponseDto>(education, "Education record created successfully"));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiResponse<string>(ex.Message));
            }
        }
        [HttpPut("{id}")]
        [SwaggerOperation(
            Summary = "Update an education record",
            Description = "Updates an existing education record with new details.",
            OperationId = "UpdateEducation"
        )]
        [SwaggerResponse(200, "Education record updated successfully", typeof(ApiResponse<EducationResponseDto>))]
        [SwaggerResponse(404, "Education record not found", typeof(ApiResponse<string>))]
        [SwaggerResponse(400, "Invalid input or validation failed", typeof(ApiResponse<string>))]
        [SwaggerResponse(401, "User is not authorized", typeof(ApiResponse<string>))]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateEducationDto dto)
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
            var updatedEducation = await _educationService.UpdateAsync(id, dto, userId);
            if (updatedEducation == null) return NotFound(new ApiResponse<string>("Education record not found"));
            return Ok(new ApiResponse<EducationResponseDto>(updatedEducation, "Education record updated successfully"));
        }
        [HttpDelete("{id}")]
        [SwaggerOperation(
            Summary = "Delete an education record",
            Description = "Permanently removes an education record from the user's portfolio.",
            OperationId = "DeleteEducation"
        )]
        [SwaggerResponse(200, "Education record deleted successfully", typeof(ApiResponse<bool>))]
        [SwaggerResponse(404, "Education record not found", typeof(ApiResponse<string>))]
        [SwaggerResponse(401, "User is not authorized", typeof(ApiResponse<string>))]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized(new ApiResponse<string>("User not authorized"));
            var deleted = await _educationService.DeleteAsync(id, userId);
            if (!deleted) return NotFound(new ApiResponse<string>("Education record not found"));
            return Ok(new ApiResponse<bool>(true, "Education record deleted successfully"));
        }
    }
}