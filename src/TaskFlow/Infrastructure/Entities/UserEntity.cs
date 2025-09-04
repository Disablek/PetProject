using TaskFlow.Domain.Entities.Interfaces;

namespace TaskFlow.Data.Entities
{
    public class UserEntity : ISAuditable
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string FullName { get; set; }
        public List<ProjectEntity> Projects { get; set; } = new();
        public List<TaskEntity> CreatedTasks { get; set; } = new(); 
        public List<TaskEntity> AssignedTasks { get; set; } = new(); 
    }
}
