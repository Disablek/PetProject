using TaskFlow.Domain.Entities.Enums;

namespace TaskFlow.Business.DTO.Task;
public class TaskDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Status Status { get; set; } = Status.New;
    public Priority Priority { get; set; } = Priority.Low;
    public DateTime? DueTime { get; set; }
    public DateTime? CompletedTime { get; set; }

    public Guid ProjectId { get; set; }
    public Guid? AssigneeId { get; set; }
    public Guid CreatorId { get; set; }
}
