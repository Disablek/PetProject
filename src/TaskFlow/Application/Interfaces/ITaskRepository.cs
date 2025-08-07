using TaskFlow.Data.Entities.Enums;

namespace TaskFlow.Business.Interfaces
{
    public interface ITaskRepository
    {
        public IEnumerable<object> GetAll();
        public object? GetById(Guid id);
        public IEnumerable<object> GetByProjectId(Guid projectId);
        public IEnumerable<object> GetByCreatorId(Guid creatorId);
        public IEnumerable<object> GetByAssigneeId(Guid assigneeId);
        public IEnumerable<object> GetByStatus(Status status);
        public IEnumerable<object> GetOverdueTasks();
        public void Add(object task);
        public bool Update(object updated);   
        public bool Delete(Guid id);
        public bool ReassignTask(Guid taskId, Guid newAssigneeId);
        public bool UpdateStatus(Guid taskId, Status status);
        public bool UpdatePriority(Guid taskId, Priority priority);
    }
}
