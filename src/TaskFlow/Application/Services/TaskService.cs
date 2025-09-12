using AutoMapper;
using TaskFlow.Business.DTO.Task;
using TaskFlow.Data.Entities;
using TaskFlow.Data.Repositories.Interfaces;
using TaskFlow.Domain.Entities.Enums;

namespace TaskFlow.Business.Services
{
    public class TaskService
    {
        private readonly ITasksRepository _taskRepository;
        private readonly IMapper _mapper;

        public TaskService(ITasksRepository taskRepository, IMapper mapper)
        {
            _taskRepository = taskRepository;
            _mapper = mapper;
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

        public async Task<bool> DeleteAsync(Guid id)
        {

            var deleted = await _taskRepository.DeleteAsync(id);
            return deleted;
        }




    }
}