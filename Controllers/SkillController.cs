using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using PortfolioCMS.DTOs.Skill;
using PortfolioCMS.Services.Interfaces;

namespace PortfolioCMS.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("CMSPolicy")]
    public class SkillController : ControllerBase
    {
        private readonly ISkillService _skillService;

        public SkillController(ISkillService skillService)
        {
            _skillService = skillService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var skills = await _skillService.GetAllSkillsAsync(userId);
            return Ok(skills);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var skill = await _skillService.GetSkillByIdAsync(id, userId);
            if (skill == null) return NotFound();

            return Ok(skill);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSkill([FromBody] CreateSkillDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var skill = await _skillService.CreateSkillAsync(dto, userId);
            return CreatedAtAction(nameof(GetById), new { id = skill.Id }, skill);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateSkillDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var updatedSkill = await _skillService.UpdateSkillAsync(id, dto, userId);
            if (updatedSkill == null) return NotFound();

            return Ok(updatedSkill);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var deleted = await _skillService.DeleteSkillAsync(id, userId);
            if (!deleted) return NotFound();

            return NoContent();
        }
    }
}
