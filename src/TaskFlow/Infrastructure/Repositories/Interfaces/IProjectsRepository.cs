using TaskFlow.Data.Entities;

namespace TaskFlow.Data.Repositories.Interfaces;
public interface IProjectsRepository
{
    Task<List<ProjectEntity>> GetAsync();
    Task<ProjectEntity?> GetByIdAsync(Guid id);
    Task<List<ProjectEntity>> GetWithTaskAsync();
    Task<List<ProjectEntity>> GetByPageAsync(int page, int pageSize);
    Task AddAsync(Guid id, Guid adminId, string name, string description);
    Task UpdateAsync(Guid id, Guid adminId, string name, string description);
    Task DeleteAsync(Guid id);
    Task<bool> IsUserInProjectAsync(Guid projectId, Guid userId);
    Task<bool> IsUserAdminAsync(Guid projectId, Guid userId);
    Task<List<TaskEntity>> GetProjectTasksAsync(Guid projectId);
    Task<List<UserEntity>> GetProjectUsersAsync(Guid projectId);
    Task AddUserToProjectAsync(Guid projectId, Guid userId);
    Task RemoveUserFromProjectAsync(Guid projectId, Guid userId);
    Task UpdateProjectUsersAsync(Guid projectId, List<Guid> userIds);
}

