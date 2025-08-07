using TaskFlow.Data.Entities;
using TaskFlow.Data.Entities.Enums;

namespace TaskFlow.Data.Repositories.Interfaces;

public interface ITasksRepository
{
    Task<List<TaskEntity>> GetByProject(Guid projectId);
    Task<TaskEntity?> GetById(Guid id);
    Task Add(Guid id, Guid projectId, string title, string description, DateTime? dueTime, Priority priority, Guid creatorId);
    Task Update(Guid id, string title, string description, DateTime? dueTime, Priority priority, Guid asigneeId);
    Task Delete(Guid id);
}