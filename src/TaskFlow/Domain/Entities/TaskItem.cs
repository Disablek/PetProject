namespace Domain.Entities
{
    public class TaskItem
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? CompletedAt { get; set; }
        public string Status { get; set; } = "Pending";

        public TaskItem()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
        }

        public TaskItem(string title, string? description = null, DateTime? dueDate = null)
        {
            Id = Guid.NewGuid();
            Title = title;
            Description = description;
            DueDate = dueDate;
            CreatedAt = DateTime.UtcNow;
        }

        public void MarkCompleted()
        {
            IsCompleted = true;
            Status = "Completed";
            CompletedAt = DateTime.UtcNow;
        }

        public void MarkInProgress()
        {
            Status = "InProgress";
        }

        public void MarkPending()
        {
            Status = "Pending";
        }
    }
}
