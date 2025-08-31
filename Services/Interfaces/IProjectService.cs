using PortfolioCMS.DTOs.Project;
using PortfolioCMS.DTOs.Projects;

namespace PortfolioCMS.Services.Interfaces
{
    public interface IProjectService
    {
        Task<IEnumerable<ProjectResponseDto>> GetAllProjectsAsync(string userId);
        Task<ProjectResponseDto?> GetProjectByIdAsync(int id, string userId);
        Task<ProjectResponseDto> CreateProjectAsync(CreateProjectDto createProjectDto, string userId);
        Task<ProjectResponseDto?> UpdateProjectAsync(int id, UpdateProjectDto updateProjectDto, string userId);
        Task<bool> DeleteProjectAsync(int id, string userId);
    }
}