using TaskFlow.Domain.Entities.Enums;

namespace TaskFlow.Business.DTO.Task;
public class CreateTaskDto
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Priority Priority { get; set; } = Priority.Low;
    public DateTime? DueTime { get; set; }
    public Guid ProjectId { get; set; }
    public Guid? AssigneeId { get; set; }
    public Guid CreatorId { get; set; } // Добавляем обязательное поле создателя
}
