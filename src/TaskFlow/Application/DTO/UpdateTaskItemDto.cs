using TaskFlow.Data.Entities.Enums;

namespace TaskFlow.Business.DTO
{
    public class UpdateTaskItemDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Status Status { get; set; }
        public Priority Priority { get; set; }
        public DateTime? DueTime { get; set; }
        
        // Связи
        public Guid AssigneeId { get; set; }
    }
}
