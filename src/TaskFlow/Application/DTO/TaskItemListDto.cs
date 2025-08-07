using TaskFlow.Data.Entities.Enums;

namespace TaskFlow.Business.DTO
{
    public class TaskItemListDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public Status Status { get; set; }
        public Priority Priority { get; set; }
        public DateTime? DueTime { get; set; }
        public DateTime CreatedAt { get; set; }

        // Связи
        public Guid ProjectId { get; set; }
        public string ProjectName { get; set; } = string.Empty;
        public Guid CreatorId { get; set; }
        public string CreatorName { get; set; } = string.Empty;
        public Guid AssigneeId { get; set; }
        public string AssigneeName { get; set; } = string.Empty;

        // Вычисляемые свойства
        public bool IsCompleted => Status == Status.Done;
        public bool IsOverdue => DueTime.HasValue && DueTime < DateTime.UtcNow && Status != Status.Done;

        public static TaskItemListDto FromEntity(TaskFlow.Business.Entities.TaskItem entity)
        {
            return new TaskItemListDto
            {
                Id = entity.Id,
                Title = entity.Title,
                Status = entity.Status,
                Priority = entity.Priority,
                DueTime = entity.DueTime,
                CreatedAt = entity.CreatedAt,
                ProjectId = entity.ProjectId,
                CreatorId = entity.CreatorId,
                AssigneeId = entity.AssigneeId
            };
        }

        public static TaskItemListDto FromInfrastructureEntity(TaskFlow.Data.Entities.TaskEntity entity)
        {
            return new TaskItemListDto
            {
                Id = entity.Id,
                Title = entity.Title,
                Status = entity.Status,
                Priority = entity.Priority,
                DueTime = entity.DueTime,
                CreatedAt = entity.CreatedAt,
                ProjectId = entity.ProjectId,
                ProjectName = entity.Project?.Name ?? string.Empty,
                CreatorId = entity.CreatorId,
                CreatorName = entity.Creator?.UserName ?? string.Empty,
                AssigneeId = entity.AssigneeId,
                AssigneeName = entity.Assignee?.UserName ?? string.Empty
            };
        }
    }
}
