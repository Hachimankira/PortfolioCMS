using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using PortfolioCMS.DTOs;
using PortfolioCMS.Services.Interfaces;

namespace PortfolioCMS.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("CMSPolicy")]
    public class EducationController : ControllerBase
    {
        private readonly IEducationService _educationService;

        public EducationController(IEducationService educationService)
        {
            _educationService = educationService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();
            var educations = await _educationService.GetAllAsync(userId);
            return Ok(educations);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();
            var education = await _educationService.GetByIdAsync(id, userId);
            if (education == null) return NotFound();
            return Ok(education);
        }
        [HttpPost]
        public async Task<IActionResult> CreateEducation([FromBody] CreateEducationDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState); // Returns validation errors

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();
            try
            {
                var education = await _educationService.CreateAsync(dto, userId);
                return CreatedAtAction(nameof(GetById), new { id = education.Id }, education);
            }
            catch (ArgumentException ex)
            {

                return BadRequest(ex.Message);
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateEducationDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState); // Returns validation errors

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();
            var updatedEducation = await _educationService.UpdateAsync(id, dto, userId);
            if (updatedEducation == null) return NotFound();
            return Ok(updatedEducation);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();
            var deleted = await _educationService.DeleteAsync(id, userId);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}