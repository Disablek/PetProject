using TaskFlow.Domain.Entities.Enums;

namespace TaskFlow.Business.DTO.Task;
public class TaskListItemDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public Status Status { get; set; }
    public Priority Priority { get; set; }
    public string? AssigneeName { get; set; }
}
