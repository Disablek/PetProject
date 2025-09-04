using TaskFlow.Domain.Entities.Enums;

namespace TaskFlow.Business.DTO.Task;
public class TaskPreviewDto
{
    public string Title { get; set; } = string.Empty;
    public DateTime? DueTime { get; set; }
    public Status Status { get; set; } = Status.New;
    public Priority Priority { get; set; } = Priority.Low;
}
