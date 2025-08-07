using TaskFlow.Data.Entities.Interfaces;

namespace TaskFlow.Data.Entities
{
    public class ProjectEntity : IAuditable, ISoftDeletable
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public List<TaskEntity> Tasks { get; set; } = [];
        public List<UserEntity> Users { get; set; } = [];

        public Guid AdminId { get; set; }
        public UserEntity? Admin { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
