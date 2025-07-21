using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pet_project
{
    public class TaskService
    {
        private readonly List<TaskItem> _tasks = new();

        public event Action? TasksChanged;

        public IReadOnlyCollection<TaskItem> GetAll() => _tasks.AsReadOnly();

        public TaskService() { }

        public void AddNewTask(string title)
        {
            _tasks.Add(new TaskItem(title));
            TasksChanged?.Invoke();
        }

        public bool RemoveTask(Guid id)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == id);
            if (task == null) return false;
            _tasks.Remove(task);
            TasksChanged?.Invoke();
            return true;
        }

        public bool CompleteTask(Guid id)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == id);
            if (task == null || task.IsCompleted) return false;
            task.MarkCompleted();
            TasksChanged?.Invoke();
            return true;
        }
    }
}
