using TaskFlow.Business.DTO.Project;
using TaskFlow.Business.DTO.Task;
using TaskFlow.Business.DTO.User;

namespace TaskFlow.Business.Services.Interfaces
{
    public interface IProjectService
    {
        Task<List<ProjectDto>> GetAllAsync();
        Task<ProjectDto?> GetByIdAsync(Guid id);
        Task<ProjectDto> CreateAsync(CreateProjectDto dto);
        Task<ProjectDto?> UpdateAsync(Guid id, UpdateProjectDto dto);
        Task<bool> DeleteAsync(Guid id);
        Task<List<TaskListItemDto>> GetProjectTasksAsync(Guid projectId);
        Task<ProjectDto> AddUserToProjectAsync(Guid projectId, Guid userId);
        Task<ProjectDto> RemoveUserFromProjectAsync(Guid projectId, Guid userId);
        Task<List<UserDto>> GetProjectUsersAsync(Guid projectId);
        Task<bool> IsUserAdminAsync(Guid projectId, Guid userId);
        Task<bool> IsUserInProjectAsync(Guid projectId, Guid userId);
    }
}

