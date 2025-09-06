using TaskFlow.Data.Entities;
using TaskFlow.Domain.Entities.Enums;

namespace TaskFlow.Data.Repositories.Interfaces;

public interface ITasksRepository
{
    Task<List<TaskEntity>> GetAllTasksAsync();
    Task<List<TaskEntity>> GetByProjectAsync(Guid projectId);
    Task<TaskEntity?> GetByIdAsync(Guid id);
    Task AddAsync(Guid id, Guid projectId, string title, string description, DateTime? dueTime, Priority priority, Guid creatorId);
    Task UpdateAsync(Guid id, string title, string description, DateTime? dueTime, Priority priority, Guid asigneeId);
    Task DeleteAsync(Guid id);
}