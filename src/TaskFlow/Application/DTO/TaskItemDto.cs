using TaskFlow.Data.Entities.Enums;

namespace TaskFlow.Business.DTO
{
    public class TaskItemDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Status Status { get; set; }
        public Priority Priority { get; set; }
        public DateTime? DueTime { get; set; }
        public DateTime? CompletedTime { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }


        public Guid ProjectId { get; set; }
        public string ProjectName { get; set; } = string.Empty;


        public Guid CreatorId { get; set; }
        public string CreatorName { get; set; } = string.Empty;
        public Guid AssigneeId { get; set; }
        public string AssigneeName { get; set; } = string.Empty;


        public bool IsCompleted => Status == Status.Done;
        public bool IsOverdue => DueTime.HasValue && DueTime < DateTime.UtcNow && Status != Status.Done;

    
        public static TaskItemDto FromEntity(TaskFlow.Business.Entities.TaskItem entity)
        {
            return new TaskItemDto
            {
                Id = entity.Id,
                Title = entity.Title,
                Description = entity.Description,
                Status = entity.Status,
                Priority = entity.Priority,
                DueTime = entity.DueTime,
                CompletedTime = entity.CompletedTime,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt,
                ProjectId = entity.ProjectId,
                CreatorId = entity.CreatorId,
                AssigneeId = entity.AssigneeId
            };
        }


        public static TaskItemDto FromInfrastructureEntity(TaskFlow.Data.Entities.TaskEntity entity)
        {
            return new TaskItemDto
            {
                Id = entity.Id,
                Title = entity.Title,
                Description = entity.Description,
                Status = entity.Status,
                Priority = entity.Priority,
                DueTime = entity.DueTime,
                CompletedTime = entity.CompletedTime,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt,
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
