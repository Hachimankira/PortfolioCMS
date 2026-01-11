using PortfolioCMS.DTOs;
using PortfolioCMS.DTOs.Project;
using PortfolioCMS.DTOs.Projects;
using PortfolioCMS.Models.Wrappers;

namespace PortfolioCMS.Services.Interfaces
{
    public interface IProjectService
    {
        Task<PagedResult<ProjectResponseDto>> GetAllProjectsAsync(string userId, PaginationFilter filter);
        Task<ProjectResponseDto?> GetProjectByIdAsync(int id, string userId);
        Task<ProjectResponseDto> CreateProjectAsync(CreateProjectDto createProjectDto, string userId);
        Task<ProjectResponseDto?> UpdateProjectAsync(int id, UpdateProjectDto updateProjectDto, string userId);
        Task<bool> DeleteProjectAsync(int id, string userId);
    }
}