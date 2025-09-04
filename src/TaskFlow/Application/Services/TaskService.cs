//using TaskFlow.Business.DTO;
//using TaskFlow.Business.Interfaces;
//using TaskFlow.Business.Entities;
//using TaskFlow.Data.Entities.Enums;

//namespace TaskFlow.Business.Services
//{
//    public class TaskService
//    {
//        private readonly ITaskRepository _taskRepository;

//        public TaskService(ITaskRepository taskRepository)
//        {
//            _taskRepository = taskRepository;
//        }

//        public IEnumerable<TaskItemListDto> GetAllTasks()
//        {
//            var tasks = _taskRepository.GetAll().Cast<TaskItem>();
//            return tasks.Select(TaskItemListDto.FromEntity);
//        }

//        public TaskItemDto? GetTaskById(Guid id)
//        {
//            var task = _taskRepository.GetById(id) as TaskItem;
//            return task != null ? TaskItemDto.FromEntity(task) : null;
//        }

//        public TaskItemDto CreateTask(CreateTaskItemDto createDto)
//        {
//            var task = new TaskItem(
//                createDto.Title, 
//                createDto.Description, 
//                createDto.CreatorId, 
//                createDto.AssigneeId, 
//                createDto.ProjectId, 
//                createDto.DueTime
//            );
            
//            task.UpdatePriority(createDto.Priority);
//            _taskRepository.Add(task);
//            return TaskItemDto.FromEntity(task);
//        }

//        public TaskItemDto? UpdateTask(Guid id, UpdateTaskItemDto updateDto)
//        {
//            var existingTask = _taskRepository.GetById(id) as TaskItem;
//            if (existingTask == null)
//                return null;

//            existingTask.Title = updateDto.Title;
//            existingTask.Description = updateDto.Description;
//            existingTask.UpdateDueTime(updateDto.DueTime);
//            existingTask.UpdatePriority(updateDto.Priority);

//            // Обновляем статус
//            switch (updateDto.Status)
//            {
//                case Status.Done:
//                    existingTask.MarkCompleted();
//                    break;
//                case Status.InProgress:
//                    existingTask.MarkInProgress();
//                    break;
//                case Status.Review:
//                    existingTask.MarkReview();
//                    break;
//                case Status.Blocked:
//                    existingTask.MarkBlocked();
//                    break;
//                case Status.New:
//                    existingTask.MarkPending();
//                    break;
//            }

//            // Переназначаем исполнителя если изменился
//            if (existingTask.AssigneeId != updateDto.AssigneeId)
//            {
//                existingTask.Reassign(updateDto.AssigneeId);
//            }

//            var success = _taskRepository.Update(existingTask);
//            return success ? TaskItemDto.FromEntity(existingTask) : null;
//        }

//        public bool DeleteTask(Guid id)
//        {
//            return _taskRepository.Delete(id);
//        }

//        public TaskItemDto? MarkTaskCompleted(Guid id)
//        {
//            var task = _taskRepository.GetById(id) as TaskItem;
//            if (task == null)
//                return null;

//            task.MarkCompleted();
//            var success = _taskRepository.Update(task);
//            return success ? TaskItemDto.FromEntity(task) : null;
//        }

//        public TaskItemDto? MarkTaskInProgress(Guid id)
//        {
//            var task = _taskRepository.GetById(id) as TaskItem;
//            if (task == null)
//                return null;

//            task.MarkInProgress();
//            var success = _taskRepository.Update(task);
//            return success ? TaskItemDto.FromEntity(task) : null;
//        }

//        public TaskItemDto? MarkTaskReview(Guid id)
//        {
//            var task = _taskRepository.GetById(id) as TaskItem;
//            if (task == null)
//                return null;

//            task.MarkReview();
//            var success = _taskRepository.Update(task);
//            return success ? TaskItemDto.FromEntity(task) : null;
//        }

//        public TaskItemDto? MarkTaskBlocked(Guid id)
//        {
//            var task = _taskRepository.GetById(id) as TaskItem;
//            if (task == null)
//                return null;

//            task.MarkBlocked();
//            var success = _taskRepository.Update(task);
//            return success ? TaskItemDto.FromEntity(task) : null;
//        }

//        public TaskItemDto? ReassignTask(Guid id, Guid newAssigneeId)
//        {
//            var task = _taskRepository.GetById(id) as TaskItem;
//            if (task == null)
//                return null;

//            task.Reassign(newAssigneeId);
//            var success = _taskRepository.Update(task);
//            return success ? TaskItemDto.FromEntity(task) : null;
//        }

//        public TaskItemDto? UpdateTaskPriority(Guid id, Priority priority)
//        {
//            var task = _taskRepository.GetById(id) as TaskItem;
//            if (task == null)
//                return null;

//            task.UpdatePriority(priority);
//            var success = _taskRepository.Update(task);
//            return success ? TaskItemDto.FromEntity(task) : null;
//        }

//        public TaskItemDto? UpdateTaskDueTime(Guid id, DateTime? dueTime)
//        {
//            var task = _taskRepository.GetById(id) as TaskItem;
//            if (task == null)
//                return null;

//            task.UpdateDueTime(dueTime);
//            var success = _taskRepository.Update(task);
//            return success ? TaskItemDto.FromEntity(task) : null;
//        }

//        // Новые методы для получения задач по различным критериям
//        public IEnumerable<TaskItemListDto> GetTasksByProject(Guid projectId)
//        {
//            var tasks = _taskRepository.GetByProjectId(projectId).Cast<TaskItem>();
//            return tasks.Select(TaskItemListDto.FromEntity);
//        }

//        public IEnumerable<TaskItemListDto> GetTasksByCreator(Guid creatorId)
//        {
//            var tasks = _taskRepository.GetByCreatorId(creatorId).Cast<TaskItem>();
//            return tasks.Select(TaskItemListDto.FromEntity);
//        }

//        public IEnumerable<TaskItemListDto> GetTasksByAssignee(Guid assigneeId)
//        {
//            var tasks = _taskRepository.GetByAssigneeId(assigneeId).Cast<TaskItem>();
//            return tasks.Select(TaskItemListDto.FromEntity);
//        }

//        public IEnumerable<TaskItemListDto> GetTasksByStatus(Status status)
//        {
//            var tasks = _taskRepository.GetByStatus(status).Cast<TaskItem>();
//            return tasks.Select(TaskItemListDto.FromEntity);
//        }

//        public IEnumerable<TaskItemListDto> GetOverdueTasks()
//        {
//            var tasks = _taskRepository.GetOverdueTasks().Cast<TaskItem>();
//            return tasks.Select(TaskItemListDto.FromEntity);
//        }

//        public IEnumerable<TaskItemListDto> GetUserTasks(Guid userId)
//        {
//            var createdTasks = _taskRepository.GetByCreatorId(userId).Cast<TaskItem>();
//            var assignedTasks = _taskRepository.GetByAssigneeId(userId).Cast<TaskItem>();
            
//            var allTasks = createdTasks.Concat(assignedTasks).DistinctBy(t => t.Id);
//            return allTasks.Select(TaskItemListDto.FromEntity);
//        }
//    }
//} 