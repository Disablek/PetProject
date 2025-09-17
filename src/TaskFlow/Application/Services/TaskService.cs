using AutoMapper;
using TaskFlow.Business.DTO.Task;
using TaskFlow.Business.Services.Interfaces;
using TaskFlow.Data.Entities;
using TaskFlow.Data.Repositories.Interfaces;
using TaskFlow.Domain.Entities.Enums;

namespace TaskFlow.Business.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITasksRepository _taskRepository;
        private readonly IProjectsRepository _projectsRepository;
        private readonly IMapper _mapper;

        public TaskService(ITasksRepository taskRepository, IMapper mapper, IProjectsRepository projectsRepository)
        {
            _taskRepository = taskRepository;
            _mapper = mapper;
            _projectsRepository = projectsRepository;
        }

        public async Task<List<TaskListItemDto>> GetAllAsync()
        {
            var entities = await _taskRepository.GetAllTasksAsync();
            if (entities == null || entities.Count == 0)
                return new List<TaskListItemDto>();

            return _mapper.Map<List<TaskListItemDto>>(entities);
        }

        public async Task<TaskDto?> GetByIdAsync(Guid id)
        {
            var entity = await _taskRepository.GetByIdAsync(id);
            if (entity == null)
                return null;
            return _mapper.Map<TaskDto>(entity);
        }

        public async Task<List<TaskListItemDto>> GetByProjectAsync(Guid projectId)
        {
            var entities = await _taskRepository.GetByProjectAsync(projectId);
            if (entities == null || entities.Count == 0)
                return new List<TaskListItemDto>();
            return _mapper.Map<List<TaskListItemDto>>(entities);
        }

        public async Task<TaskDto> CreateAsync(CreateTaskDto dto)
        {
            var entity = _mapper.Map<TaskEntity>(dto);
            var createdEntity = await _taskRepository.AddAsync(entity);
            return _mapper.Map<TaskDto>(createdEntity);
        }

        public async Task<TaskDto?> UpdateAsync(Guid id, UpdateTaskDto dto)
        {
            var entity = await _taskRepository.GetTrackedByIdAsync(id);
            if (entity == null) return null;

            // Применяем только поля, которые пришли (PATCH semantics)
            if (dto.Title is not null) entity.Title = dto.Title;
            if (dto.Description is not null) entity.Description = dto.Description;
            if (dto.DueTime.HasValue) entity.DueTime = dto.DueTime;
            if (dto.Priority.HasValue) entity.Priority = dto.Priority.Value;

            // Assignee: если пришёл guid => назначаем; если явный null — снимаем
            if (dto.AssigneeId is not null)
                entity.AssigneeId = dto.AssigneeId;

            // Статус и CompletedTime логика:
            if (dto.Status.HasValue && dto.Status.Value != entity.Status)
            {
                var newStatus = dto.Status.Value;
                if (newStatus == Status.Done && entity.Status != Status.Done)
                {
                    entity.CompletedTime = DateTime.UtcNow;
                }
                else if (entity.Status == Status.Done && newStatus != Status.Done)
                {
                    entity.CompletedTime = null;
                }

                entity.Status = newStatus;
            }

            var updated = await _taskRepository.UpdateAsync(entity);

            return _mapper.Map<TaskDto>(updated);
        }

        public async Task<bool> DeleteAsync(Guid taskId, Guid currentUserId)
        {
            var task = await _taskRepository.GetByIdAsync(taskId);
            if (task == null)
                return false;

            if (task.CreatorId == currentUserId)
            {
                var isInProject = await _projectsRepository.IsUserInProjectAsync(task.ProjectId, currentUserId);
                if (isInProject)
                {
                    return await _taskRepository.DeleteAsync(taskId);
                }
            }

            var isAdmin = await _projectsRepository.IsUserAdminAsync(task.ProjectId, currentUserId);
            if (isAdmin)
            {
                return await _taskRepository.DeleteAsync(taskId);
            }

            throw new Exception("Нет прав на удаление этой задачи"); 
        }

        public async Task<TaskDto?> ChangeStatusAsync(Guid id, Status newStatus, Guid performedBy, TaskDto taskDto)
        {
            var task = await _taskRepository.GetByIdAsync(id);
            if (task == null)
                return null;

            if (task.AssigneeId == null)
                task.AssigneeId = performedBy;

            if (task.AssigneeId == performedBy)
            {
                var isInProject = await _projectsRepository.IsUserInProjectAsync(task.ProjectId, performedBy);
                if (isInProject)
                {
                    task.Status = newStatus;
                    return _mapper.Map<TaskDto?>( await _taskRepository.UpdateAsync(task));
                }
                else
                    throw new Exception("Пользователь не находится в проекте");
            }
            else if (await _projectsRepository.IsUserAdminAsync(task.ProjectId, performedBy))
            {
                task.Status = newStatus;
                return _mapper.Map<TaskDto?>(await _taskRepository.UpdateAsync(task));
            }
            else
                throw new Exception("Нет прав на изменение этой задачи");
        }

        public async Task<TaskDto?> AssignAsync(Guid id, Guid assigneeId, Guid changedBy)
        {
            var task = await _taskRepository.GetByIdAsync(id);
            if (task == null) return null;
            if (task.AssigneeId == null)
            {
                task.AssigneeId = assigneeId;
                return _mapper.Map<TaskDto?>(await _taskRepository.UpdateAsync(task));
            }

            if (await _projectsRepository.IsUserAdminAsync(task.ProjectId,changedBy))
            {
                task.AssigneeId = assigneeId;
                return _mapper.Map<TaskDto?>(await _taskRepository.UpdateAsync(task));
            }

            throw new Exception("Невозможно изменить исполнителя");
        }



    }
}