using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortfolioCMS.Models.DTOs;
using PortfolioCMS.Services.Interfaces;
using System.Security.Claims;

namespace PortfolioCMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Assumes authentication is required
    public class SocialLinksController : ControllerBase
    {
        private readonly ISocialLinksService _service;

        public SocialLinksController(ISocialLinksService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();
            var links = await _service.GetAllAsync(userId);
            return Ok(links);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();
            var link = await _service.GetByIdAsync(id, userId);
            if (link == null) return NotFound();
            return Ok(link);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateLinkDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();
            var link = await _service.CreateAsync(dto, userId);
            return CreatedAtAction(nameof(GetById), new { id = link.Id }, link);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateLinkDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            dto.Id = id; // Ensure ID matches route
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();
            var link = await _service.UpdateAsync(dto, userId);
            if (link == null) return NotFound();
            return Ok(link);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();
            var success = await _service.DeleteAsync(id, userId);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}