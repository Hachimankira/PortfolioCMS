using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using PortfolioCMS.Models.DTOs;
using PortfolioCMS.DTOs;
using PortfolioCMS.Models.Wrappers;
using PortfolioCMS.Services.Interfaces;
using System.Security.Claims;

namespace PortfolioCMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Assumes authentication is required
    [EnableCors("CMSPolicy")]
    [SwaggerTag("Manage social media links")]
    public class SocialLinksController : ControllerBase
    {
        private readonly ISocialLinksService _service;

        public SocialLinksController(ISocialLinksService service)
        {
            _service = service;
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Get all social links",
            Description = "Retrieves a paginated list of social media links for the authenticated user.",
            OperationId = "GetSocialLinks"
        )]
        [SwaggerResponse(200, "Returns a paginated list of social links", typeof(PagedResponse<IEnumerable<LinkResponseDto>>))]
        [SwaggerResponse(401, "User is not authorized", typeof(ApiResponse<string>))]
        public async Task<IActionResult> GetAll([FromQuery] PaginationFilter filter)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized(new ApiResponse<string>("User not authorized"));
            var result = await _service.GetAllAsync(userId, filter);
            return Ok(new PagedResponse<IEnumerable<LinkResponseDto>>(result.Items, filter.PageNumber, filter.PageSize, result.TotalCount));
        }

        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = "Get social link by ID",
            Description = "Retrieves a specific social link by its unique identifier.",
            OperationId = "GetSocialLinkById"
        )]
        [SwaggerResponse(200, "Returns the requested social link", typeof(ApiResponse<LinkResponseDto>))]
        [SwaggerResponse(404, "Social link not found", typeof(ApiResponse<string>))]
        [SwaggerResponse(401, "User is not authorized", typeof(ApiResponse<string>))]
        public async Task<IActionResult> GetById(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized(new ApiResponse<string>("User not authorized"));
            var link = await _service.GetByIdAsync(id, userId);
            if (link == null) return NotFound(new ApiResponse<string>("Social link not found"));
            return Ok(new ApiResponse<LinkResponseDto>(link, "Social link retrieved successfully"));
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Create a new social link",
            Description = "Adds a new social media link to the user's portfolio.",
            OperationId = "CreateSocialLink"
        )]
        [SwaggerResponse(201, "Social link created successfully", typeof(ApiResponse<LinkResponseDto>))]
        [SwaggerResponse(400, "Invalid input or validation failed", typeof(ApiResponse<string>))]
        [SwaggerResponse(401, "User is not authorized", typeof(ApiResponse<string>))]
        public async Task<IActionResult> Create([FromBody] CreateLinkDto dto)
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
            var link = await _service.CreateAsync(dto, userId);
            return CreatedAtAction(nameof(GetById), new { id = link.Id }, new ApiResponse<LinkResponseDto>(link, "Social link created successfully"));
        }

        [HttpPut("{id}")]
        [SwaggerOperation(
            Summary = "Update a social link",
            Description = "Updates an existing social link with new details.",
            OperationId = "UpdateSocialLink"
        )]
        [SwaggerResponse(200, "Social link updated successfully", typeof(ApiResponse<LinkResponseDto>))]
        [SwaggerResponse(404, "Social link not found", typeof(ApiResponse<string>))]
        [SwaggerResponse(400, "Invalid input or validation failed", typeof(ApiResponse<string>))]
        [SwaggerResponse(401, "User is not authorized", typeof(ApiResponse<string>))]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateLinkDto dto)
        {
            if (!ModelState.IsValid) 
            {
                 var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                 var response = new ApiResponse<string>("Validation failed");
                 response.Errors = errors;
                 return BadRequest(response); // Returns validation errors wrapped
            }
            dto.Id = id; // Ensure ID matches route
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized(new ApiResponse<string>("User not authorized"));
            var link = await _service.UpdateAsync(dto, userId);
            if (link == null) return NotFound(new ApiResponse<string>("Social link not found"));
            return Ok(new ApiResponse<LinkResponseDto>(link, "Social link updated successfully"));
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(
            Summary = "Delete a social link",
            Description = "Permanently removes a social link from the user's portfolio.",
            OperationId = "DeleteSocialLink"
        )]
        [SwaggerResponse(200, "Social link deleted successfully", typeof(ApiResponse<bool>))]
        [SwaggerResponse(404, "Social link not found", typeof(ApiResponse<string>))]
        [SwaggerResponse(401, "User is not authorized", typeof(ApiResponse<string>))]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized(new ApiResponse<string>("User not authorized"));
            var success = await _service.DeleteAsync(id, userId);
            if (!success) return NotFound(new ApiResponse<string>("Social link not found"));
            return Ok(new ApiResponse<bool>(true, "Social link deleted successfully"));
        }
    }
}