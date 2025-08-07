using TaskFlow.Data.Entities.Enums;

namespace TaskFlow.Business.Entities
{
    public class TaskItem
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Status Status { get; set; } = Status.New;
        public Priority Priority { get; set; } = Priority.Low;
        public DateTime? DueTime { get; set; }
        public DateTime? CompletedTime { get; set; }

        // Связи с проектом
        public Guid ProjectId { get; set; }

        // Связи с пользователями
        public Guid CreatorId { get; set; }
        public Guid AssigneeId { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public TaskItem()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public TaskItem(string title, string description, Guid creatorId, Guid assigneeId, Guid projectId, DateTime? dueTime = null)
        {
            Id = Guid.NewGuid();
            Title = title;
            Description = description;
            CreatorId = creatorId;
            AssigneeId = assigneeId;
            ProjectId = projectId;
            DueTime = dueTime;
            Status = Status.New;
            Priority = Priority.Medium;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public void MarkCompleted()
        {
            Status = Status.Done;
            CompletedTime = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public void MarkInProgress()
        {
            Status = Status.InProgress;
            UpdatedAt = DateTime.UtcNow;
        }

        public void MarkPending()
        {
            Status = Status.New;
            UpdatedAt = DateTime.UtcNow;
        }

        public void MarkReview()
        {
            Status = Status.Review;
            UpdatedAt = DateTime.UtcNow;
        }

        public void MarkBlocked()
        {
            Status = Status.Blocked;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdatePriority(Priority priority)
        {
            Priority = priority;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateDueTime(DateTime? dueTime)
        {
            DueTime = dueTime;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Reassign(Guid newAssigneeId)
        {
            AssigneeId = newAssigneeId;
            UpdatedAt = DateTime.UtcNow;
        }

        public bool IsOverdue => DueTime.HasValue && DueTime < DateTime.UtcNow && Status != Status.Done;
        public bool IsCompleted => Status == Status.Done;
    }
} 