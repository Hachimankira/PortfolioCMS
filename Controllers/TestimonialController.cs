using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using PortfolioCMS.Models.DTOs;
using PortfolioCMS.Services.Interfaces;
using System.Security.Claims;

namespace PortfolioCMS.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("CMSPolicy")]
    public class TestimonialsController : ControllerBase
    {
        private readonly ITestimonialService _service;

        public TestimonialsController(ITestimonialService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();
            var testimonials = await _service.GetAllAsync(userId);
            return Ok(testimonials);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();
            var testimonial = await _service.GetByIdAsync(id, userId);
            if (testimonial == null) return NotFound();
            return Ok(testimonial);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTestimonialDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();
            var testimonial = await _service.CreateAsync(dto, userId);
            return CreatedAtAction(nameof(GetById), new { id = testimonial.Id }, testimonial);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateTestimonialDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            dto.Id = id; // Ensure ID matches route
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();
            var result = await _service.UpdateAsync(dto, userId);
            if (result == null) return NotFound();
            return NoContent();
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