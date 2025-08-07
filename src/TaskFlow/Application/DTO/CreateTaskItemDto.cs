using TaskFlow.Data.Entities.Enums;

namespace TaskFlow.Business.DTO
{
    public class CreateTaskItemDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Priority Priority { get; set; } = Priority.Medium;
        public DateTime? DueTime { get; set; }
        
        // Связи
        public Guid ProjectId { get; set; }
        public Guid CreatorId { get; set; }
        public Guid AssigneeId { get; set; }
    }
}
