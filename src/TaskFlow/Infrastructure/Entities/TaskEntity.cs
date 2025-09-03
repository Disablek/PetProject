using TaskFlow.Data.Entities.Enums;
using TaskFlow.Data.Entities.Interfaces;

namespace TaskFlow.Data.Entities
{
    public class TaskEntity : ISSoftDeletable
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Status Status { get; set; } = Status.New;
        public Priority Priority { get; set; } = Priority.Low;
        public DateTime? DueTime { get; set; } 
        public DateTime? CompletedTime { get; set; }


        public Guid ProjectId { get; set; }
        public ProjectEntity? Project { get; set; }

        public Guid CreatorId { get; set; }
        public UserEntity? Creator { get; set; }

        public Guid AssigneeId { get; set; } // Исполнитель
        public UserEntity? Assignee { get; set; }
    }
}
