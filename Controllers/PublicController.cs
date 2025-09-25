using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using PortfolioCMS.Services.Interfaces;
using PortfolioCMS.Models;
using PortfolioCMS.Attributes;
using PortfolioCMS.Models.DTOs;
using Microsoft.AspNetCore.RateLimiting;

namespace PortfolioCMS.Controllers
{
    [EnableRateLimiting("PublicApiRateLimitPolicy")]
    [ApiController]
    [Route("public")]
    [EnableCors("PublicApiPolicy")]
    [ApiKey]
    public class PublicController : ControllerBase
    {
        private readonly IProjectService _projectService;
        private readonly ISkillService _skillService;
        private readonly IExperienceService _experienceService;
        private readonly IEducationService _educationService;
        private readonly ITestimonialService _testimonialService;
        private readonly ICertification _certificationService;
        private readonly ISocialLinksService _socialLinksService;
        private readonly UserManager<ApplicationUser> _userManager;

        public PublicController(
            IProjectService projectService,
            ISkillService skillService,
            IExperienceService experienceService,
            IEducationService educationService,
            ITestimonialService testimonialService,
            ICertification certificationService,
            ISocialLinksService socialLinksService,
            UserManager<ApplicationUser> userManager)
        {
            _projectService = projectService;
            _skillService = skillService;
            _experienceService = experienceService;
            _educationService = educationService;
            _testimonialService = testimonialService;
            _certificationService = certificationService;
            _socialLinksService = socialLinksService;
            _userManager = userManager;
        }

        [HttpGet("{username}/projects")]
        public async Task<IActionResult> GetProjects(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null) return NotFound(new { message = "User not found" });

            // The response DTOs should already exclude sensitive information
            var projects = await _projectService.GetAllProjectsAsync(user.Id);
            return Ok(projects);
        }

        [HttpGet("{username}/skills")]
        public async Task<IActionResult> GetSkills(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null) return NotFound(new { message = "User not found" });

            var skills = await _skillService.GetAllSkillsAsync(user.Id);
            return Ok(skills);
        }

        [HttpGet("{username}/profile")]
        public async Task<IActionResult> GetProfile(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null) return NotFound(new { message = "User not found" });

            // Create a profile DTO - you might already have a ProfileResponseDto
            var profile = new
            {
                FullName = user.FullName,
                Headline = user.Headline,
                Summary = user.Summary,
                Location = user.Location,
                ProfilePictureUrl = user.ProfilePictureUrl
            };

            return Ok(profile);
        }

        [HttpGet("{username}/experiences")]
        public async Task<IActionResult> GetExperiences(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null) return NotFound(new { message = "User not found" });

            var experiences = await _experienceService.GetAllExperiencesAsync(user.Id);
            return Ok(experiences);
        }

        [HttpGet("{username}/education")]
        public async Task<IActionResult> GetEducation(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null) return NotFound(new { message = "User not found" });

            var education = await _educationService.GetAllAsync(user.Id);
            return Ok(education);
        }

        [HttpGet("{username}/certifications")]
        public async Task<IActionResult> GetCertifications(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null) return NotFound(new { message = "User not found" });

            var certifications = await _certificationService.GetAllAsync(user.Id);
            return Ok(certifications);
        }

        [HttpGet("{username}/testimonials")]
        public async Task<IActionResult> GetTestimonials(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null) return NotFound(new { message = "User not found" });

            var testimonials = await _testimonialService.GetAllAsync(user.Id);

            // For testimonials, you might still want to filter only approved ones
            var approvedTestimonials = testimonials.Where(t => t.IsApproved);
            return Ok(approvedTestimonials);
        }

        [HttpGet("{username}/sociallinks")]
        public async Task<IActionResult> GetSocialLinks(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null) return NotFound(new { message = "User not found" });

            var socialLinks = await _socialLinksService.GetAllAsync(user.Id);
            return Ok(socialLinks);
        }

        [HttpGet("{username}/all")]
        public async Task<IActionResult> GetAllPortfolioData(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null) return NotFound(new { message = "User not found" });

            // Profile data
            var profile = new
            {
                FullName = user.FullName,
                Headline = user.Headline,
                Summary = user.Summary,
                Location = user.Location,
                ProfilePictureUrl = user.ProfilePictureUrl
            };

            // Get all data using the service methods that return DTOs
            var projects = await _projectService.GetAllProjectsAsync(user.Id);
            var skills = await _skillService.GetAllSkillsAsync(user.Id);
            var experiences = await _experienceService.GetAllExperiencesAsync(user.Id);
            var education = await _educationService.GetAllAsync(user.Id);
            var certifications = await _certificationService.GetAllAsync(user.Id);
            var testimonials = await _testimonialService.GetAllAsync(user.Id);
            var socialLinks = await _socialLinksService.GetAllAsync(user.Id);

            // Filter approved testimonials
            var approvedTestimonials = testimonials.Where(t => t.IsApproved);

            // Return all data in one response
            return Ok(new
            {
                profile,
                projects,
                skills,
                experiences,
                education,
                certifications,
                testimonials = approvedTestimonials,
                socialLinks
            });
        }
    }
}