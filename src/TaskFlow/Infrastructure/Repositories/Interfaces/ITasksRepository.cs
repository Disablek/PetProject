using TaskFlow.Data.Entities;
using TaskFlow.Domain.Entities.Enums;

namespace TaskFlow.Data.Repositories.Interfaces;

public interface ITasksRepository
{
    Task<List<TaskEntity>> GetAllTasksAsync();
    Task<List<TaskEntity>> GetByProjectAsync(Guid projectId);
    Task<TaskEntity?> GetByIdAsync(Guid id);
    Task<TaskEntity?> GetTrackedByIdAsync(Guid id);
    Task<TaskEntity> AddAsync(TaskEntity entity);
    Task<TaskEntity> UpdateAsync(TaskEntity entity);
    Task<bool> DeleteAsync(Guid id);
    
    // Методы для работы с пользователями (для мок-логики)
    Task<List<UserEntity>> GetAllUsersAsync();
    Task AddUserAsync(UserEntity user);
}