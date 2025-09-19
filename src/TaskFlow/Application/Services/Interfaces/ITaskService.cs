using TaskFlow.Business.DTO.Task;
using TaskFlow.Domain.Entities.Enums;

namespace TaskFlow.Business.Services.Interfaces
{
    public interface ITaskService
    {
        Task<List<TaskListItemDto>> GetAllAsync();
        Task<TaskDto?> GetByIdAsync(Guid id);
        Task<List<TaskListItemDto>> GetByProjectAsync(Guid projectId);
        Task<TaskDto> CreateAsync(CreateTaskDto dto);
        Task<TaskDto?> UpdateAsync(Guid id, UpdateTaskDto dto);
        Task<bool> DeleteAsync(Guid taskId, Guid currentUserId);
        Task<TaskDto?> ChangeStatusAsync(Guid id, Status newStatus, Guid performedBy);
        Task<TaskDto?> AssignAsync(Guid id, Guid assigneeId, Guid changedBy);
        Task<TaskDto?> UpdatePriorityAsync(Guid id, Priority priority, Guid currentUserId);
        Task<TaskDto?> UpdateDueTimeAsync(Guid id, DateTime? dueTime, Guid currentUserId);

    }
}