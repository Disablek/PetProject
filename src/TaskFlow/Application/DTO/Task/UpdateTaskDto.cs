using TaskFlow.Domain.Entities.Enums;

namespace TaskFlow.Business.DTO.Task;
public class UpdateTaskDto
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Priority Priority { get; set; }
    public Status Status { get; set; }
    public DateTime? DueTime { get; set; }
    public Guid? AssigneeId { get; set; }
}
