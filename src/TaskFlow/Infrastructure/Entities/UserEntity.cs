using TaskFlow.Domain.Entities.Interfaces;

namespace TaskFlow.Data.Entities
{
    public class UserEntity : ISAuditable
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Email { get; set; }
        public string FullName { get; set; }
        public ICollection<ProjectEntity> Projects { get; set; } = new HashSet<ProjectEntity>();
        public ICollection<TaskEntity> CreatedTasks { get; set; } = new HashSet<TaskEntity>(); 
        public ICollection<TaskEntity> AssignedTasks { get; set; } = new HashSet<TaskEntity>(); 
    }
}
