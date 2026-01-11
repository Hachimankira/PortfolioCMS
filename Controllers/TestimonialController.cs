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
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("CMSPolicy")]
    [SwaggerTag("Manage client testimonials")]
    public class TestimonialsController : ControllerBase
    {
        private readonly ITestimonialService _service;

        public TestimonialsController(ITestimonialService service)
        {
            _service = service;
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Get all testimonials",
            Description = "Retrieves a paginated list of testimonials for the authenticated user.",
            OperationId = "GetTestimonials"
        )]
        [SwaggerResponse(200, "Returns a paginated list of testimonials", typeof(PagedResponse<IEnumerable<TestimonialResponseDto>>))]
        [SwaggerResponse(401, "User is not authorized", typeof(ApiResponse<string>))]
        public async Task<IActionResult> GetAll([FromQuery] PaginationFilter filter)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized(new ApiResponse<string>("User not authorized"));
            var result = await _service.GetAllAsync(userId, filter);
            return Ok(new PagedResponse<IEnumerable<TestimonialResponseDto>>(result.Items, filter.PageNumber, filter.PageSize, result.TotalCount));
        }

        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = "Get testimonial by ID",
            Description = "Retrieves a specific testimonial by its unique identifier.",
            OperationId = "GetTestimonialById"
        )]
        [SwaggerResponse(200, "Returns the requested testimonial", typeof(ApiResponse<TestimonialResponseDto>))]
        [SwaggerResponse(404, "Testimonial not found", typeof(ApiResponse<string>))]
        [SwaggerResponse(401, "User is not authorized", typeof(ApiResponse<string>))]
        public async Task<IActionResult> GetById(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized(new ApiResponse<string>("User not authorized"));
            var testimonial = await _service.GetByIdAsync(id, userId);
            if (testimonial == null) return NotFound(new ApiResponse<string>("Testimonial not found"));
            return Ok(new ApiResponse<TestimonialResponseDto>(testimonial, "Testimonial retrieved successfully"));
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Create a new testimonial",
            Description = "Adds a new client testimonial to the user's portfolio.",
            OperationId = "CreateTestimonial"
        )]
        [SwaggerResponse(201, "Testimonial created successfully", typeof(ApiResponse<TestimonialResponseDto>))]
        [SwaggerResponse(400, "Invalid input or validation failed", typeof(ApiResponse<string>))]
        [SwaggerResponse(401, "User is not authorized", typeof(ApiResponse<string>))]
        public async Task<IActionResult> Create([FromBody] CreateTestimonialDto dto)
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
            var testimonial = await _service.CreateAsync(dto, userId);
            return CreatedAtAction(nameof(GetById), new { id = testimonial.Id }, new ApiResponse<TestimonialResponseDto>(testimonial, "Testimonial created successfully"));
        }

        [HttpPut("{id}")]
        [SwaggerOperation(
            Summary = "Update a testimonial",
            Description = "Updates an existing client testimonial with new details.",
            OperationId = "UpdateTestimonial"
        )]
        [SwaggerResponse(200, "Testimonial updated successfully", typeof(ApiResponse<TestimonialResponseDto>))]
        [SwaggerResponse(404, "Testimonial not found", typeof(ApiResponse<string>))]
        [SwaggerResponse(400, "Invalid input or validation failed", typeof(ApiResponse<string>))]
        [SwaggerResponse(401, "User is not authorized", typeof(ApiResponse<string>))]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateTestimonialDto dto)
        {
            if (!ModelState.IsValid) 
            {
                 var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                 var response = new ApiResponse<string>("Validation failed");
                 response.Errors = errors;
                 return BadRequest(response);
            }

            dto.Id = id; // Ensure ID matches route
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized(new ApiResponse<string>("User not authorized"));
            var result = await _service.UpdateAsync(dto, userId);
            if (result == null) return NotFound(new ApiResponse<string>("Testimonial not found"));
            return Ok(new ApiResponse<TestimonialResponseDto>(result, "Testimonial updated successfully"));
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(
            Summary = "Delete a testimonial",
            Description = "Permanently removes a testimonial from the user's portfolio.",
            OperationId = "DeleteTestimonial"
        )]
        [SwaggerResponse(200, "Testimonial deleted successfully", typeof(ApiResponse<bool>))]
        [SwaggerResponse(404, "Testimonial not found", typeof(ApiResponse<string>))]
        [SwaggerResponse(401, "User is not authorized", typeof(ApiResponse<string>))]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized(new ApiResponse<string>("User not authorized"));
            var success = await _service.DeleteAsync(id, userId);
            if (!success) return NotFound(new ApiResponse<string>("Testimonial not found"));
            return Ok(new ApiResponse<bool>(true, "Testimonial deleted successfully"));
        }
    }
}