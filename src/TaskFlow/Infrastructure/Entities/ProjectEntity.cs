using TaskFlow.Domain.Entities.Interfaces;


namespace TaskFlow.Data.Entities
{
    public class ProjectEntity : ISAuditable
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public ICollection<TaskEntity> Tasks { get; set; } = new HashSet<TaskEntity>();
        public ICollection<UserEntity> Users { get; set; } = new HashSet<UserEntity>();

        public Guid AdminId { get; set; }
        public UserEntity? Admin { get; set; }
    }
}
