using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using PortfolioCMS.DTOs.Project;
using PortfolioCMS.DTOs.Projects;
using PortfolioCMS.Services.Interfaces;

namespace PortfolioCMS.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("CMSPolicy")] // Use the restricted CORS policy

    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;
        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();
            var projects = await _projectService.GetAllProjectsAsync(userId);
            return Ok(projects);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();
            var project = await _projectService.GetProjectByIdAsync(id, userId);
            if (project == null) return NotFound();
            return Ok(project);
        }
        [HttpPost]
        public async Task<IActionResult> CreateProject([FromBody] CreateProjectDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var project = await _projectService.CreateProjectAsync(dto, userId);
            return CreatedAtAction(nameof(GetById), new { id = project.Id }, project);

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProject(int id, [FromBody] UpdateProjectDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var updatedProject = await _projectService.UpdateProjectAsync(id, dto, userId);
            return Ok(updatedProject);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var deleted = await _projectService.DeleteProjectAsync(id, userId);
            if (!deleted) return NotFound();
            return NoContent();
        }

    }
}