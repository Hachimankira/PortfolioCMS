using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using PortfolioCMS.Services.Interfaces;
using PortfolioCMS.Models; // Adjust this namespace if ApplicationUser is in a different namespace
using PortfolioCMS.Attributes; // Assuming ApiKey attribute is in this namespace

namespace PortfolioCMS.Controllers
{
    [ApiController]
    [Route("public")]
    [EnableCors("PublicApiPolicy")] // Use the public CORS policy
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
            // Get the username from the middleware
            var authenticatedUsername = HttpContext.Items["Username"]?.ToString();

            // Get the user by username
            var user = await _userManager.FindByNameAsync(username);
            if (user == null) return NotFound(new { message = "User not found" });

            // Fetch projects for the user
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

        // [HttpGet("{username}/profile")]
        // public async Task<IActionResult> GetProfile(string username)
        // {
        //     var user = await _userManager.FindByNameAsync(username);
        //     if (user == null) return NotFound(new { message = "User not found" });

        //     // Return only public profile information
        //     var profile = new
        //     {
        //         FullName = user.FullName,
        //         Headline = user.Headline,
        //         Summary = user.Summary,
        //         Location = user.Location,
        //         ProfilePictureUrl = user.ProfilePictureUrl
        //         // Don't include email, phone, or other sensitive data
        //     };

        //     return Ok(profile);
        // }

        // Additional endpoints for other entities (education, experience, etc.)
        // Follow the same pattern of fetching by username and filtering sensitive data
    }
}