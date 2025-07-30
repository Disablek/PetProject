namespace TaskService.Api.Features.Tasks
{
    public interface ITaskRepository
    {
        public IEnumerable<TaskItem> GetAll();
        public TaskItem? GetById(Guid id);
        public void Add(TaskItem task);
        public bool Update(TaskItem updated);
        public bool Delete(Guid id);
    }
}
