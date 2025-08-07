namespace TaskFlow.Data.Entities
{
    public class UserEntity
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty; 

        public List<ProjectEntity> Projects { get; set; } = [];
        public List<TaskEntity> CreatedTasks { get; set; } = []; 
        public List<TaskEntity> AssignedTasks { get; set; } = []; 

        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        public DateTime? DeletedAt { get; private set; }
    }
}
