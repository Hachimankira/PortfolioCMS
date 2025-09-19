using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using PortfolioCMS.DTOs;
using PortfolioCMS.Services.Interfaces;

namespace PortfolioCMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    [EnableCors("CMSPolicy")]
    public class CertificationController : ControllerBase
    {
        private readonly ICertification _certificationService;

        public CertificationController(ICertification certificationService)
        {
            _certificationService = certificationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();
            var certifications = await _certificationService.GetAllAsync(userId);
            return Ok(certifications);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();
            var certification = await _certificationService.GetByIdAsync(id, userId);
            if (certification == null) return NotFound();
            return Ok(certification);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCertification([FromBody] CreateCertificationDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState); // Returns validation errors

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            try
            {
                var certification = await _certificationService.CreateAsync(dto, userId);
                return CreatedAtAction(nameof(GetById), new { id = certification.Id }, certification);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateCertificationDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState); // Returns validation errors

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();
            var result = await _certificationService.UpdateAsync(dto, id, userId);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();
            var result = await _certificationService.DeleteAsync(id, userId);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}