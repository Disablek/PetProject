using AutoMapper;
using TaskFlow.Business.DTO.Project;
using TaskFlow.Business.DTO.Task;
using TaskFlow.Business.DTO.User;
using TaskFlow.Business.Exceptions;
using TaskFlow.Business.Services.Interfaces;
using TaskFlow.Data.Entities;
using TaskFlow.Data.Repositories.Interfaces;

namespace TaskFlow.Business.Services
{
    public class ProjectService(
        IProjectsRepository projectRepository,
        IUserRepository userRepository,
        IMapper mapper) : IProjectService
    {
        public async Task<List<ProjectDto>> GetAllAsync()
        {
            var entities = await projectRepository.GetAsync();
            return mapper.Map<List<ProjectDto>>(entities);
        }

        public async Task<ProjectDto?> GetByIdAsync(Guid id)
        {
            var entity = await projectRepository.GetByIdAsync(id);
            return entity == null ? null : mapper.Map<ProjectDto>(entity);
        }

        public async Task<ProjectDto> CreateAsync(CreateProjectDto dto)
        {
            var entity = mapper.Map<ProjectEntity>(dto);
            entity.Id = Guid.NewGuid();
            
            await projectRepository.AddAsync(entity.Id, entity.AdminId, entity.Name, entity.Description);
            
            // Добавляем пользователей в проект если они указаны
            if (dto.UserIds != null && dto.UserIds.Any())
            {
                foreach (var userId in dto.UserIds)
                {
                    await projectRepository.AddUserToProjectAsync(entity.Id, userId);
                }
            }
            
            var createdEntity = await projectRepository.GetByIdAsync(entity.Id);
            return mapper.Map<ProjectDto>(createdEntity!);
        }

        public async Task<ProjectDto?> UpdateAsync(Guid id, UpdateProjectDto dto)
        {
            var entity = await projectRepository.GetByIdAsync(id);
            if (entity == null)
                throw new NotFoundException($"Проект с ID {id} не найден");

            await projectRepository.UpdateAsync(id, dto.AdminId, dto.Name, dto.Description);
            
            // Обновляем участников проекта
            await projectRepository.UpdateProjectUsersAsync(id, dto.UserIds);
            
            var updatedEntity = await projectRepository.GetByIdAsync(id);
            return mapper.Map<ProjectDto>(updatedEntity!);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await projectRepository.GetByIdAsync(id);
            if (entity == null)
                throw new NotFoundException($"Проект с ID {id} не найден");

            await projectRepository.DeleteAsync(id);
            return true;
        }

        public async Task<List<TaskListItemDto>> GetProjectTasksAsync(Guid projectId)
        {
            var project = await projectRepository.GetByIdAsync(projectId);
            if (project == null)
                throw new NotFoundException($"Проект с ID {projectId} не найден");

            var tasks = await projectRepository.GetProjectTasksAsync(projectId);
            return mapper.Map<List<TaskListItemDto>>(tasks);
        }

        public async Task<ProjectDto> AddUserToProjectAsync(Guid projectId, Guid userId)
        {
            var project = await projectRepository.GetByIdAsync(projectId);
            if (project == null)
                throw new NotFoundException($"Проект с ID {projectId} не найден");

            var user = await userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new NotFoundException($"Пользователь с ID {userId} не найден");

            await projectRepository.AddUserToProjectAsync(projectId, userId);
            
            var updatedProject = await projectRepository.GetByIdAsync(projectId);
            return mapper.Map<ProjectDto>(updatedProject!);
        }

        public async Task<ProjectDto> RemoveUserFromProjectAsync(Guid projectId, Guid userId)
        {
            var project = await projectRepository.GetByIdAsync(projectId);
            if (project == null)
                throw new NotFoundException($"Проект с ID {projectId} не найден");

            await projectRepository.RemoveUserFromProjectAsync(projectId, userId);
            
            var updatedProject = await projectRepository.GetByIdAsync(projectId);
            return mapper.Map<ProjectDto>(updatedProject!);
        }

        public async Task<List<UserDto>> GetProjectUsersAsync(Guid projectId)
        {
            var project = await projectRepository.GetByIdAsync(projectId);
            if (project == null)
                throw new NotFoundException($"Проект с ID {projectId} не найден");

            var users = await projectRepository.GetProjectUsersAsync(projectId);
            return mapper.Map<List<UserDto>>(users);
        }

        public async Task<bool> IsUserAdminAsync(Guid projectId, Guid userId)
        {
            return await projectRepository.IsUserAdminAsync(projectId, userId);
        }

        public async Task<bool> IsUserInProjectAsync(Guid projectId, Guid userId)
        {
            return await projectRepository.IsUserInProjectAsync(projectId, userId);
        }
    }
}

