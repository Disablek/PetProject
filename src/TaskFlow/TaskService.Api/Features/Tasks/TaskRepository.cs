namespace TaskService.Api.Features.Tasks
{
    public class TaskRepository
    {
        private readonly List<TaskItem> _tasks = new();

        public IEnumerable<TaskItem> GetAll() => _tasks;

        public TaskItem? GetById(Guid id) => _tasks.FirstOrDefault(t => t.Id == id);

        public void Add(TaskItem task)
        {
            task.Id = Guid.NewGuid();
            _tasks.Add(task);
        }

        public bool Update(TaskItem updated)
        {
            var existing = GetById(updated.Id);
            if (existing == null) return false;
            existing.Title = updated.Title;
            existing.Description = updated.Description;
            existing.DueDate = updated.DueDate;
            existing.Status = updated.Status;
            return true;
        }

        public bool Delete(Guid id)
        {
            var task = GetById(id);
            if (task == null) return false;
            _tasks.Remove(task);
            return true;
        }
    }
}
