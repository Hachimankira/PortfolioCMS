using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortfolioCMS.DTOs.Experience;
using PortfolioCMS.Services.Interfaces;

namespace PortfolioCMS.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ExperienceController : ControllerBase
    {
        private readonly IExperienceService _experienceService;
        public ExperienceController(IExperienceService experienceService)
        {
            _experienceService = experienceService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();
            var experiences = await _experienceService.GetAllExperiencesAsync(userId);
            return Ok(experiences);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();
            var experience = await _experienceService.GetExperienceByIdAsync(id, userId);
            if (experience == null) return NotFound();
            return Ok(experience);
        }
        [HttpPost]
        public async Task<IActionResult> CreateExperience([FromBody] CreateExperienceDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState); // Returns validation errors

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();
            var experience = await _experienceService.AddExperienceAsync(dto, userId);
            return CreatedAtAction(nameof(GetById), new { id = experience.Id }, experience);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateExperienceDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState); // Returns validation errors

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();
            var updatedExperience = await _experienceService.UpdateExperienceAsync(id, dto, userId);
            if (updatedExperience == null) return NotFound();
            return Ok(updatedExperience);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();
            var deleted = await _experienceService.DeleteExperienceAsync(id, userId);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}