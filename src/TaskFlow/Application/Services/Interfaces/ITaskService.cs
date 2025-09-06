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
        Task<bool> DeleteAsync(Guid id);

        Task<TaskDto?> ChangeStatusAsync(Guid id, Status newStatus, Guid performedBy);
        Task<TaskDto?> AssignAsync(Guid id, Guid? assigneeId);
        Task<TaskDto?> UpdatePriorityAsync(Guid id, Priority priority);
        Task<TaskDto?> UpdateDueTimeAsync(Guid id, DateTime? dueTime);
    }
}