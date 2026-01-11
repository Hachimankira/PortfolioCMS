using Microsoft.AspNetCore.Cors;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using PortfolioCMS.Services.Interfaces;
using PortfolioCMS.Models;
using PortfolioCMS.Attributes;
using PortfolioCMS.Models.DTOs;
using Microsoft.AspNetCore.RateLimiting;
using PortfolioCMS.Models.Wrappers;
using PortfolioCMS.DTOs;
using PortfolioCMS.DTOs.Projects;
using PortfolioCMS.DTOs.Skill;
using PortfolioCMS.DTOs.Experience;

namespace PortfolioCMS.Controllers
{
    [EnableRateLimiting("PublicApiRateLimitPolicy")]
    [ApiController]
    [Route("public")]
    [EnableCors("PublicApiPolicy")]
    [ApiKey]
    [SwaggerTag("Public API endpoints for fetching portfolio data via API Key")]
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
        [SwaggerOperation(Summary = "Get public projects", Description = "Retrieves projects for a specific user.")]
        [SwaggerResponse(200, "Returns list of projects", typeof(ApiResponse<IEnumerable<ProjectResponseDto>>))]
        [SwaggerResponse(404, "User not found", typeof(ApiResponse<string>))]
        public async Task<IActionResult> GetProjects(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null) return NotFound(new ApiResponse<string>("User not found"));

            // The response DTOs should already exclude sensitive information
            // For Public API, we currently default to first 100 projects if not specified. 
            // TODO: Add pagination support to public endpoints
            var result = await _projectService.GetAllProjectsAsync(user.Id, new PaginationFilter(1, 100));
            return Ok(new ApiResponse<object>(result.Items, "Projects retrieved successfully"));
        }

        [HttpGet("{username}/skills")]
        [SwaggerOperation(Summary = "Get public skills", Description = "Retrieves skills for a specific user.")]
        [SwaggerResponse(200, "Returns list of skills", typeof(ApiResponse<IEnumerable<SkillResponseDto>>))]
        [SwaggerResponse(404, "User not found", typeof(ApiResponse<string>))]
        public async Task<IActionResult> GetSkills(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null) return NotFound(new ApiResponse<string>("User not found"));

            var result = await _skillService.GetAllSkillsAsync(user.Id, new PaginationFilter(1, 100));
            return Ok(new ApiResponse<object>(result.Items, "Skills retrieved successfully"));
        }

        [HttpGet("{username}/profile")]
        [SwaggerOperation(Summary = "Get public profile", Description = "Retrieves public profile information.")]
        [SwaggerResponse(200, "Returns public profile", typeof(ApiResponse<object>))]
        [SwaggerResponse(404, "User not found", typeof(ApiResponse<string>))]
        public async Task<IActionResult> GetProfile(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null) return NotFound(new ApiResponse<string>("User not found"));

            // Create a profile DTO - you might already have a ProfileResponseDto
            var profile = new
            {
                FullName = user.FullName,
                Headline = user.Headline,
                Summary = user.Summary,
                Location = user.Location,
                ProfilePictureUrl = user.ProfilePictureUrl
            };

            return Ok(new ApiResponse<object>(profile, "Profile retrieved successfully"));
        }

        [HttpGet("{username}/experiences")]
        [SwaggerOperation(Summary = "Get public experiences", Description = "Retrieves experiences for a specific user.")]
        [SwaggerResponse(200, "Returns list of experiences", typeof(ApiResponse<IEnumerable<ExperienceResponseDto>>))]
        [SwaggerResponse(404, "User not found", typeof(ApiResponse<string>))]
        public async Task<IActionResult> GetExperiences(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null) return NotFound(new ApiResponse<string>("User not found"));

            var result = await _experienceService.GetAllExperiencesAsync(user.Id, new PaginationFilter(1, 100));
            return Ok(new ApiResponse<object>(result.Items, "Experiences retrieved successfully"));
        }

        [HttpGet("{username}/education")]
        [SwaggerOperation(Summary = "Get public education", Description = "Retrieves education history for a specific user.")]
        [SwaggerResponse(200, "Returns list of education records", typeof(ApiResponse<IEnumerable<EducationResponseDto>>))]
        [SwaggerResponse(404, "User not found", typeof(ApiResponse<string>))]
        public async Task<IActionResult> GetEducation(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null) return NotFound(new ApiResponse<string>("User not found"));

            var result = await _educationService.GetAllAsync(user.Id, new PaginationFilter(1, 100));
            return Ok(new ApiResponse<object>(result.Items, "Education retrieved successfully"));
        }

        [HttpGet("{username}/certifications")]
        [SwaggerOperation(Summary = "Get public certifications", Description = "Retrieves certifications for a specific user.")]
        [SwaggerResponse(200, "Returns list of certifications", typeof(ApiResponse<IEnumerable<CertificationResponseDto>>))]
        [SwaggerResponse(404, "User not found", typeof(ApiResponse<string>))]
        public async Task<IActionResult> GetCertifications(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null) return NotFound(new ApiResponse<string>("User not found"));

            var result = await _certificationService.GetAllAsync(user.Id, new PaginationFilter(1, 100));
            return Ok(new ApiResponse<object>(result.Items, "Certifications retrieved successfully"));
        }

        [HttpGet("{username}/testimonials")]
        [SwaggerOperation(Summary = "Get public testimonials", Description = "Retrieves approved testimonials for a specific user.")]
        [SwaggerResponse(200, "Returns list of testimonials", typeof(ApiResponse<IEnumerable<TestimonialResponseDto>>))]
        [SwaggerResponse(404, "User not found", typeof(ApiResponse<string>))]
        public async Task<IActionResult> GetTestimonials(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null) return NotFound(new ApiResponse<string>("User not found"));

            var result = await _testimonialService.GetAllAsync(user.Id, new PaginationFilter(1, 100));

            // For testimonials, you might still want to filter only approved ones
            var approvedTestimonials = result.Items.Where(t => t.IsApproved);
            return Ok(new ApiResponse<object>(approvedTestimonials, "Testimonials retrieved successfully"));
        }

        [HttpGet("{username}/sociallinks")]
        [SwaggerOperation(Summary = "Get public social links", Description = "Retrieves social links for a specific user.")]
        [SwaggerResponse(200, "Returns list of social links", typeof(ApiResponse<IEnumerable<LinkResponseDto>>))]
        [SwaggerResponse(404, "User not found", typeof(ApiResponse<string>))]
        public async Task<IActionResult> GetSocialLinks(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null) return NotFound(new ApiResponse<string>("User not found"));

            var result = await _socialLinksService.GetAllAsync(user.Id, new PaginationFilter(1, 100));
            return Ok(new ApiResponse<object>(result.Items, "Social links retrieved successfully"));
        }

        [HttpGet("{username}/all")]
        [SwaggerOperation(Summary = "Get all portfolio data", Description = "Retrieves all public portfolio data for a specific user in a single request.")]
        [SwaggerResponse(200, "Returns all portfolio data", typeof(ApiResponse<object>))]
        [SwaggerResponse(404, "User not found", typeof(ApiResponse<string>))]
        public async Task<IActionResult> GetAllPortfolioData(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null) return NotFound(new ApiResponse<string>("User not found"));

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
            var projectsResult = await _projectService.GetAllProjectsAsync(user.Id, new PaginationFilter(1, 1000));
            var projects = projectsResult.Items;
            
            var skillsResult = await _skillService.GetAllSkillsAsync(user.Id, new PaginationFilter(1, 1000));
            var skills = skillsResult.Items;
            
            var experiencesResult = await _experienceService.GetAllExperiencesAsync(user.Id, new PaginationFilter(1, 1000));
            var experiences = experiencesResult.Items;
            
            var educationResult = await _educationService.GetAllAsync(user.Id, new PaginationFilter(1, 1000));
            var education = educationResult.Items;
            
            var certificationsResult = await _certificationService.GetAllAsync(user.Id, new PaginationFilter(1, 1000));
            var certifications = certificationsResult.Items;
            
            var result = await _testimonialService.GetAllAsync(user.Id, new PaginationFilter(1, 1000));
            var testimonials = result.Items;
            
            var socialLinksResult = await _socialLinksService.GetAllAsync(user.Id, new PaginationFilter(1, 1000));
            var socialLinks = socialLinksResult.Items;

            // Filter approved testimonials
            var approvedTestimonials = testimonials.Where(t => t.IsApproved);

            // Return all data in one response
            return Ok(new ApiResponse<object>(new
            {
                profile,
                projects,
                skills,
                experiences,
                education,
                certifications,
                testimonials = approvedTestimonials,
                socialLinks
            }, "All portfolio data retrieved successfully"));
        }
    }
}