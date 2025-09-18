using AutoMapper;
using TaskFlow.Business.DTO.Task;
using TaskFlow.Business.Exceptions;
using TaskFlow.Business.Services.Interfaces;
using TaskFlow.Data.Entities;
using TaskFlow.Data.Repositories.Interfaces;
using TaskFlow.Domain.Entities.Enums;

namespace TaskFlow.Business.Services
{
    public class TaskService(ITasksRepository taskRepository, IMapper mapper, IProjectsRepository projectsRepository)
        : ITaskService
    {
        public async Task<List<TaskListItemDto>> GetAllAsync()
        {
            var entities = await taskRepository.GetAllTasksAsync();
            return mapper.Map<List<TaskListItemDto>>(entities);
        }

        public async Task<TaskDto?> GetByIdAsync(Guid id)
        {
            var entity = await taskRepository.GetByIdAsync(id)
                ?? throw new NotFoundException($"Задача с ID {id} не найдена");
            
            return mapper.Map<TaskDto>(entity);
        }

        public async Task<List<TaskListItemDto>> GetByProjectAsync(Guid projectId)
        {
            var entities = await taskRepository.GetByProjectAsync(projectId)
                ?? throw new NotFoundException($"Проект с ID {projectId} не найден");
            return mapper.Map<List<TaskListItemDto>>(entities);
        }

        public async Task<TaskDto> CreateAsync(CreateTaskDto dto)
        {
            // Если CreatorId не указан, используем первого пользователя из базы как мок
            if (dto.CreatorId == Guid.Empty)
            {
                // Получаем первого пользователя из базы как мок-создателя
                var mockUser = await GetMockUserAsync();
                dto.CreatorId = mockUser.Id;
            }

            var entity = mapper.Map<TaskEntity>(dto);
            var createdEntity = await taskRepository.AddAsync(entity);
            return mapper.Map<TaskDto>(createdEntity);
        }

        private async Task<UserEntity> GetMockUserAsync()
        {
            // Получаем первого пользователя из базы как мок-создателя
            var users = await taskRepository.GetAllUsersAsync();
            if (users.Any())
            {
                return users.First();
            }
            
            // Если пользователей нет, создаем мок-пользователя
            var mockUser = new UserEntity
            {
                Id = Guid.NewGuid(),
                UserName = "mock_user",
                FullName = "Mock User",
                Email = "mock@taskflow.com",
                PasswordHash = "mock_hash"
            };
            
            await taskRepository.AddUserAsync(mockUser);
            return mockUser;
        }

        public async Task<TaskDto?> UpdateAsync(Guid id, UpdateTaskDto dto)
        {
            var entity = await taskRepository.GetTrackedByIdAsync(id)
                ?? throw new Exception("Not found");

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

            var updated = await taskRepository.UpdateAsync(entity);

            return mapper.Map<TaskDto>(updated);
        }

        public async Task<bool> DeleteAsync(Guid taskId, Guid currentUserId)
        {
            var task = await taskRepository.GetByIdAsync(taskId)
                ?? throw new Exception("Not found");

            if (task.CreatorId == currentUserId)
            {
                var isInProject = await projectsRepository.IsUserInProjectAsync(task.ProjectId, currentUserId);
                if (isInProject)
                {
                    return await taskRepository.DeleteAsync(taskId);
                }
            }

            var isAdmin = await projectsRepository.IsUserAdminAsync(task.ProjectId, currentUserId);
            if (isAdmin)
            {
                return await taskRepository.DeleteAsync(taskId);
            }
            else
            {
                return await taskRepository.DeleteAsync(taskId); // Временно
            }

            throw new Exception("Нет прав на удаление этой задачи"); 
        }

        public async Task<TaskDto?> ChangeStatusAsync(Guid id, Status newStatus, Guid performedBy, TaskDto taskDto)
        {
            var task = await taskRepository.GetByIdAsync(id)
                ?? throw new Exception("Not found");

            if (task.AssigneeId == null)
                task.AssigneeId = performedBy;

            // if (task.AssigneeId == performedBy)
            // {
                // var isInProject = await projectsRepository.IsUserInProjectAsync(task.ProjectId, performedBy);
                // if (isInProject)
                // {
                    task.Status = newStatus;
                    await taskRepository.Test(id,2);
                    return mapper.Map<TaskDto?>(task);
        }
                // else
                // {
                //     throw new Exception("Пользователь не находится в проекте");
                // }
            // }
            // if (await projectsRepository.IsUserAdminAsync(task.ProjectId, performedBy))
            // {
            //     task.Status = newStatus;
            //     return mapper.Map<TaskDto?>(await taskRepository.UpdateAsync(task));
            // }
            // else
            //     throw new Exception("Нет прав на изменение этой задачи");
        
    

        public async Task<TaskDto?> AssignAsync(Guid id, Guid assigneeId, Guid changedBy)
        {
            var task = await taskRepository.GetByIdAsync(id)
                ?? throw new Exception("Пользователь не находится в проекте");

            if (task.AssigneeId == null)
            {
                task.AssigneeId = assigneeId;
                return mapper.Map<TaskDto?>(await taskRepository.UpdateAsync(task));
            }

            if (await projectsRepository.IsUserAdminAsync(task.ProjectId,changedBy))
            {
                task.AssigneeId = assigneeId;
                return mapper.Map<TaskDto?>(await taskRepository.UpdateAsync(task));
            }

            throw new Exception("Невозможно изменить исполнителя");
        }

        public async Task<TaskDto?> UpdatePriorityAsync(Guid id, Priority priority, Guid currentUserId)
        {
            var task = await taskRepository.GetByIdAsync(id)
                ?? throw new Exception("Пользователь не находится в проекте");


            if (task.CreatorId == currentUserId ||
                await projectsRepository.IsUserAdminAsync(task.ProjectId, currentUserId))
            {
                task.Priority = priority;
                return mapper.Map<TaskDto?>(await taskRepository.UpdateAsync(task));
            }
            else
            {
                throw new Exception("Нет прав на изменение этой задачи");
            }
        }

        public Task<TaskDto?> UpdateDueTimeAsync(Guid id, DateTime? dueTime)
        {
            throw new NotImplementedException();
        }
    }
}