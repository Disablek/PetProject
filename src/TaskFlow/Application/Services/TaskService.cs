using Application.DTO;
using Application.Interfaces;
using Domain.Entities;

namespace Application.Services
{
    public class TaskService
    {
        private readonly ITaskRepository _taskRepository;

        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public IEnumerable<TaskItemListDto> GetAllTasks()
        {
            var tasks = _taskRepository.GetAll();
            return tasks.Select(TaskItemListDto.FromEntity);
        }

        public TaskItemDto? GetTaskById(Guid id)
        {
            var task = _taskRepository.GetById(id);
            return task != null ? TaskItemDto.FromEntity(task) : null;
        }

        public TaskItemDto CreateTask(CreateTaskItemDto createDto)
        {
            var task = new TaskItem(createDto.Title, createDto.Description, createDto.DueDate);
            _taskRepository.Add(task);
            return TaskItemDto.FromEntity(task);
        }

        public TaskItemDto? UpdateTask(Guid id, UpdateTaskItemDto updateDto)
        {
            var existingTask = _taskRepository.GetById(id);
            if (existingTask == null)
                return null;

            existingTask.Title = updateDto.Title;
            existingTask.Description = updateDto.Description;
            existingTask.DueDate = updateDto.DueDate;
            existingTask.Status = updateDto.Status;

            if (updateDto.IsCompleted && !existingTask.IsCompleted)
            {
                existingTask.MarkCompleted();
            }
            else if (!updateDto.IsCompleted && existingTask.IsCompleted)
            {
                existingTask.IsCompleted = false;
                existingTask.CompletedAt = null;
            }

            var success = _taskRepository.Update(existingTask);
            return success ? TaskItemDto.FromEntity(existingTask) : null;
        }

        public bool DeleteTask(Guid id)
        {
            return _taskRepository.Delete(id);
        }

        public TaskItemDto? MarkTaskCompleted(Guid id)
        {
            var task = _taskRepository.GetById(id);
            if (task == null)
                return null;

            task.MarkCompleted();
            var success = _taskRepository.Update(task);
            return success ? TaskItemDto.FromEntity(task) : null;
        }
    }
} 