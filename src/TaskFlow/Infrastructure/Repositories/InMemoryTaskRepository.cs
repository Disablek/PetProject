using Application.Interfaces;
using Domain.Entities;

namespace Infrastructure.Repositories
{
    public class InMemoryTaskRepository : ITaskRepository
    {
        private readonly List<TaskItem> _tasks = new();

        public IEnumerable<TaskItem> GetAll()
        {
            return _tasks.ToList();
        }

        public TaskItem? GetById(Guid id)
        {
            return _tasks.FirstOrDefault(t => t.Id == id);
        }

        public void Add(TaskItem task)
        {
            _tasks.Add(task);
        }

        public bool Update(TaskItem updated)
        {
            var existingTask = _tasks.FirstOrDefault(t => t.Id == updated.Id);
            if (existingTask == null)
                return false;

            var index = _tasks.IndexOf(existingTask);
            _tasks[index] = updated;
            return true;
        }

        public bool Delete(Guid id)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == id);
            if (task == null)
                return false;

            return _tasks.Remove(task);
        }
    }
} 