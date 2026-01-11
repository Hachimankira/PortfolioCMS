using System.Security.Claims;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using PortfolioCMS.DTOs.Project;
using PortfolioCMS.DTOs.Projects;
using PortfolioCMS.DTOs;
using PortfolioCMS.Models.Wrappers;
using PortfolioCMS.Services.Interfaces;

namespace PortfolioCMS.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("CMSPolicy")] // Use the restricted CORS policy
    [SwaggerTag("Manage portfolio projects")]

    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;
        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Get all projects",
            Description = "Retrieves a paginated list of projects for the authenticated user.",
            OperationId = "GetProjects"
        )]
        [SwaggerResponse(200, "Returns a paginated list of projects", typeof(PagedResponse<IEnumerable<ProjectResponseDto>>))]
        [SwaggerResponse(401, "User is not authorized", typeof(ApiResponse<string>))]
        public async Task<IActionResult> GetAll([FromQuery] PaginationFilter filter)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized(new ApiResponse<string>("User not authorized"));
            var result = await _projectService.GetAllProjectsAsync(userId, filter);
            return Ok(new PagedResponse<IEnumerable<ProjectResponseDto>>(result.Items, filter.PageNumber, filter.PageSize, result.TotalCount));
        }
        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = "Get project by ID",
            Description = "Retrieves a specific project by its unique identifier.",
            OperationId = "GetProjectById"
        )]
        [SwaggerResponse(200, "Returns the requested project", typeof(ApiResponse<ProjectResponseDto>))]
        [SwaggerResponse(404, "Project not found", typeof(ApiResponse<string>))]
        [SwaggerResponse(401, "User is not authorized", typeof(ApiResponse<string>))]
        public async Task<IActionResult> GetById(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized(new ApiResponse<string>("User not authorized"));
            var project = await _projectService.GetProjectByIdAsync(id, userId);
            if (project == null) return NotFound(new ApiResponse<string>("Project not found"));
            return Ok(new ApiResponse<ProjectResponseDto>(project, "Project retrieved successfully"));
        }
        [HttpPost]
        [SwaggerOperation(
            Summary = "Create a new project",
            Description = "Adds a new project to the user's portfolio.",
            OperationId = "CreateProject"
        )]
        [SwaggerResponse(201, "Project created successfully", typeof(ApiResponse<ProjectResponseDto>))]
        [SwaggerResponse(400, "Invalid input or validation failed", typeof(ApiResponse<string>))]
        [SwaggerResponse(401, "User is not authorized", typeof(ApiResponse<string>))]
        public async Task<IActionResult> CreateProject([FromBody] CreateProjectDto dto)
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

            var project = await _projectService.CreateProjectAsync(dto, userId);
            return CreatedAtAction(nameof(GetById), new { id = project.Id }, new ApiResponse<ProjectResponseDto>(project, "Project created successfully"));

        }

        [HttpPut("{id}")]
        [SwaggerOperation(
            Summary = "Update a project",
            Description = "Updates an existing project with new details.",
            OperationId = "UpdateProject"
        )]
        [SwaggerResponse(200, "Project updated successfully", typeof(ApiResponse<ProjectResponseDto>))]
        [SwaggerResponse(404, "Project not found", typeof(ApiResponse<string>))]
        [SwaggerResponse(400, "Invalid input or validation failed", typeof(ApiResponse<string>))]
        [SwaggerResponse(401, "User is not authorized", typeof(ApiResponse<string>))]
        public async Task<IActionResult> UpdateProject(int id, [FromBody] UpdateProjectDto dto)
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

            var updatedProject = await _projectService.UpdateProjectAsync(id, dto, userId);
            if (updatedProject == null) return NotFound(new ApiResponse<string>("Project not found"));
            return Ok(new ApiResponse<ProjectResponseDto>(updatedProject, "Project updated successfully"));
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(
            Summary = "Delete a project",
            Description = "Permanently removes a project from the user's portfolio.",
            OperationId = "DeleteProject"
        )]
        [SwaggerResponse(200, "Project deleted successfully", typeof(ApiResponse<bool>))]
        [SwaggerResponse(404, "Project not found", typeof(ApiResponse<string>))]
        [SwaggerResponse(401, "User is not authorized", typeof(ApiResponse<string>))]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized(new ApiResponse<string>("User not authorized"));

            var deleted = await _projectService.DeleteProjectAsync(id, userId);
            if (!deleted) return NotFound(new ApiResponse<string>("Project not found"));
            return Ok(new ApiResponse<bool>(true, "Project deleted successfully"));
        }

    }
}