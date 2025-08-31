using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PortfolioCMS.Data;
using PortfolioCMS.DTOs.Project;
using PortfolioCMS.DTOs.Projects;
using PortfolioCMS.Models;
using PortfolioCMS.Services.Interfaces;

namespace PortfolioCMS.Services.Implementation
{
    public class ProjectService : IProjectService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public ProjectService(AppDbContext context)
        {
            _context = context;
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CreateProjectDto, Project>();
                cfg.CreateMap<UpdateProjectDto, Project>()
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) =>
                        srcMember != null &&
                        (srcMember is not string str || !string.IsNullOrEmpty(str))));  // Ignore null and empty strings
                cfg.CreateMap<Project, ProjectResponseDto>();
            });
            _mapper = new Mapper(config);
        }

        public async Task<IEnumerable<ProjectResponseDto>> GetAllProjectsAsync(string userId)
        {
            var projects = await _context.Projects
                .Where(p => p.UserId == userId)
                .OrderBy(p => p.DisplayOrder)
                .ThenBy(p => p.CreatedAt)
                .ToListAsync();
            return _mapper.Map<IEnumerable<ProjectResponseDto>>(projects);
        }

        public async Task<ProjectResponseDto?> GetProjectByIdAsync(int id, string userId)
        {
            var project = await _context.Projects
                .Where(p => p.Id == id && p.UserId == userId)
                .FirstOrDefaultAsync();
            if (project == null) return null;
            return _mapper.Map<ProjectResponseDto>(project);
        }

        public async Task<ProjectResponseDto> CreateProjectAsync(CreateProjectDto createProjectDto, string userId)
        {
            var project = _mapper.Map<Project>(createProjectDto);
            project.CreatedAt = DateTime.UtcNow;
            project.UpdatedAt = DateTime.UtcNow;
            project.UserId = userId;
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();
            return _mapper.Map<ProjectResponseDto>(project);
        }

        public async Task<ProjectResponseDto?> UpdateProjectAsync(int id, UpdateProjectDto updateProjectDto, string userId)
        {
            if (updateProjectDto == null || id == 0) return null;

            var existingProject = await _context.Projects
                .Where(p => p.Id == id && p.UserId == userId)
                .FirstOrDefaultAsync();
            if (existingProject == null) return null;

            _mapper.Map(updateProjectDto, existingProject);
            existingProject.UpdatedAt = DateTime.UtcNow;

            _context.Projects.Update(existingProject);
            await _context.SaveChangesAsync();
            return _mapper.Map<ProjectResponseDto>(existingProject);
        }

        public async Task<bool> DeleteProjectAsync(int id, string userId)
        {
            var project = await _context.Projects
                .Where(p => p.Id == id && p.UserId == userId)
                .FirstOrDefaultAsync();
            if (project == null) return false;

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}