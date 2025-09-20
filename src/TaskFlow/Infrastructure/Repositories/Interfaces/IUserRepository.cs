using TaskFlow.Data.Entities;

namespace TaskFlow.Data.Repositories.Interfaces;

public interface IUserRepository
{
    Task<List<UserEntity>> GetAsync();
    Task<UserEntity?> GetByIdAsync(Guid id);
    Task<List<UserEntity>> GetWithProjectsAsync();
    Task AddAsync(UserEntity user);
    Task AddAsync(Guid id, string userName, string passwordHash, string fullName);
    Task UpdateAsync(Guid id, string? userName, string? email, string? passwordHash);
    Task UpdateUserNameAsync(Guid id, string userName);
    Task UpdatePasswordAsync(Guid id, string passwordHash);
    Task DeleteAsync(Guid id);
    Task<List<TaskEntity>> GetUserTasksAsync(Guid userId);
    Task<List<ProjectEntity>> GetUserProjectsAsync(Guid userId);
    Task<List<TaskEntity>> GetUserCreatedTasksAsync(Guid userId);
    Task<List<TaskEntity>> GetUserAssignedTasksAsync(Guid userId);
    Task<bool> IsEmailExistsAsync(string email);
    Task<bool> IsUserNameExistsAsync(string userName);
    Task<UserEntity?> GetFirstUserAsync();
    Task<UserEntity?> GetAdminUserAsync();
}