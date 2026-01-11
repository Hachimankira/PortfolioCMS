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
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    [EnableCors("CMSPolicy")]
    [SwaggerTag("Manage user certifications")]
    public class CertificationController : ControllerBase
    {
        private readonly ICertification _certificationService;

        public CertificationController(ICertification certificationService)
        {
            _certificationService = certificationService;
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Get all certifications",
            Description = "Retrieves a paginated list of certifications for the authenticated user.",
            OperationId = "GetCertifications"
        )]
        [SwaggerResponse(200, "Returns a paginated list of certifications", typeof(PagedResponse<IEnumerable<CertificationResponseDto>>))]
        [SwaggerResponse(401, "User is not authorized", typeof(ApiResponse<string>))]
        public async Task<IActionResult> GetAll([FromQuery] PaginationFilter filter)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized(new ApiResponse<string>("User not authorized"));
            var result = await _certificationService.GetAllAsync(userId, filter);
            return Ok(new PagedResponse<IEnumerable<CertificationResponseDto>>(result.Items, filter.PageNumber, filter.PageSize, result.TotalCount));
        }

        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = "Get certification by ID",
            Description = "Retrieves a specific certification by its unique identifier.",
            OperationId = "GetCertificationById"
        )]
        [SwaggerResponse(200, "Returns the requested certification", typeof(ApiResponse<CertificationResponseDto>))]
        [SwaggerResponse(404, "Certification not found", typeof(ApiResponse<string>))]
        [SwaggerResponse(401, "User is not authorized", typeof(ApiResponse<string>))]
        public async Task<IActionResult> GetById(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized(new ApiResponse<string>("User not authorized"));
            var certification = await _certificationService.GetByIdAsync(id, userId);
            if (certification == null) return NotFound(new ApiResponse<string>("Certification not found"));
            return Ok(new ApiResponse<CertificationResponseDto>(certification, "Certification retrieved successfully"));
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Create a new certification",
            Description = "Adds a new certification to the user's portfolio.",
            OperationId = "CreateCertification"
        )]
        [SwaggerResponse(201, "Certification created successfully", typeof(ApiResponse<CertificationResponseDto>))]
        [SwaggerResponse(400, "Invalid input or validation failed", typeof(ApiResponse<string>))]
        [SwaggerResponse(401, "User is not authorized", typeof(ApiResponse<string>))]
        public async Task<IActionResult> CreateCertification([FromBody] CreateCertificationDto dto)
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
                var certification = await _certificationService.CreateAsync(dto, userId);
                return CreatedAtAction(nameof(GetById), new { id = certification.Id }, new ApiResponse<CertificationResponseDto>(certification, "Certification created successfully"));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiResponse<string>(ex.Message));
            }
        }

        [HttpPut("{id}")]
        [SwaggerOperation(
            Summary = "Update a certification",
            Description = "Updates an existing certification with new details.",
            OperationId = "UpdateCertification"
        )]
        [SwaggerResponse(200, "Certification updated successfully", typeof(ApiResponse<CertificationResponseDto>))]
        [SwaggerResponse(404, "Certification not found", typeof(ApiResponse<string>))]
        [SwaggerResponse(400, "Invalid input or validation failed", typeof(ApiResponse<string>))]
        [SwaggerResponse(401, "User is not authorized", typeof(ApiResponse<string>))]
        public async Task<IActionResult> Update(int id, UpdateCertificationDto dto)
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
            var result = await _certificationService.UpdateAsync(dto, id, userId);
            if (result == null) return NotFound(new ApiResponse<string>("Certification not found"));
            return Ok(new ApiResponse<CertificationResponseDto>(result, "Certification updated successfully"));
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(
            Summary = "Delete a certification",
            Description = "Permanently removes a certification from the user's portfolio.",
            OperationId = "DeleteCertification"
        )]
        [SwaggerResponse(200, "Certification deleted successfully", typeof(ApiResponse<bool>))]
        [SwaggerResponse(404, "Certification not found", typeof(ApiResponse<string>))]
        [SwaggerResponse(401, "User is not authorized", typeof(ApiResponse<string>))]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized(new ApiResponse<string>("User not authorized"));
            var result = await _certificationService.DeleteAsync(id, userId);
            if (!result) return NotFound(new ApiResponse<string>("Certification not found"));
            return Ok(new ApiResponse<bool>(true, "Certification deleted successfully"));
        }
    }
}