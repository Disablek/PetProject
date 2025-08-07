using TaskFlow.Business.Interfaces;
using TaskFlow.Business.Entities;
using TaskFlow.Data.Entities.Enums;

namespace TaskFlow.Business.Repositories
{
    public class InMemoryTaskRepository : ITaskRepository
    {
        private readonly List<TaskItem> _tasks = new();

        public IEnumerable<object> GetAll()
        {
            return _tasks.Where(t => t.DeletedAt == null).Cast<object>().ToList();
        }

        public object? GetById(Guid id)
        {
            return _tasks.FirstOrDefault(t => t.Id == id && t.DeletedAt == null);
        }

        public IEnumerable<object> GetByProjectId(Guid projectId)
        {
            return _tasks.Where(t => t.ProjectId == projectId && t.DeletedAt == null).Cast<object>().ToList();
        }

        public IEnumerable<object> GetByCreatorId(Guid creatorId)
        {
            return _tasks.Where(t => t.CreatorId == creatorId && t.DeletedAt == null).Cast<object>().ToList();
        }

        public IEnumerable<object> GetByAssigneeId(Guid assigneeId)
        {
            return _tasks.Where(t => t.AssigneeId == assigneeId && t.DeletedAt == null).Cast<object>().ToList();
        }

        public IEnumerable<object> GetByStatus(Status status)
        {
            return _tasks.Where(t => t.Status == status && t.DeletedAt == null).Cast<object>().ToList();
        }

        public IEnumerable<object> GetOverdueTasks()
        {
            return _tasks.Where(t => t.IsOverdue && t.DeletedAt == null).Cast<object>().ToList();
        }

        public void Add(object task)
        {
            if (task is TaskItem taskItem)
            {
                _tasks.Add(taskItem);
            }
        }

        public bool Update(object updated)
        {
            if (updated is not TaskItem taskItem)
                return false;

            var existingTask = _tasks.FirstOrDefault(t => t.Id == taskItem.Id);
            if (existingTask == null)
                return false;

            var index = _tasks.IndexOf(existingTask);
            _tasks[index] = taskItem;
            return true;
        }

        public bool Delete(Guid id)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == id);
            if (task == null)
                return false;

            // Soft delete
            task.DeletedAt = DateTime.UtcNow;
            return true;
        }

        public bool ReassignTask(Guid taskId, Guid newAssigneeId)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == taskId);
            if (task == null)
                return false;

            task.Reassign(newAssigneeId);
            return true;
        }

        public bool UpdateStatus(Guid taskId, Status status)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == taskId);
            if (task == null)
                return false;

            switch (status)
            {
                case Status.Done:
                    task.MarkCompleted();
                    break;
                case Status.InProgress:
                    task.MarkInProgress();
                    break;
                case Status.Review:
                    task.MarkReview();
                    break;
                case Status.Blocked:
                    task.MarkBlocked();
                    break;
                case Status.New:
                    task.MarkPending();
                    break;
            }

            return true;
        }

        public bool UpdatePriority(Guid taskId, Priority priority)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == taskId);
            if (task == null)
                return false;

            task.UpdatePriority(priority);
            return true;
        }
    }
} 