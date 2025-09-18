using AutoMapper;
using TaskFlow.Business.DTO.User;
using TaskFlow.Business.DTO.Task;
using TaskFlow.Business.DTO.Project;
using TaskFlow.Business.Exceptions;
using TaskFlow.Business.Services.Interfaces;
using TaskFlow.Data.Entities;
using TaskFlow.Data.Repositories.Interfaces;

namespace TaskFlow.Business.Services
{
    public class UserService(
        IUserRepository userRepository,
        ITasksRepository tasksRepository,
        IProjectsRepository projectsRepository,
        IMapper mapper) : IUserService
    {
        public async Task<List<UserDto>> GetAllAsync()
        {
            var entities = await userRepository.GetAsync();
            return mapper.Map<List<UserDto>>(entities);
        }

        public async Task<UserDto?> GetByIdAsync(Guid id)
        {
            var entity = await userRepository.GetByIdAsync(id);
            return entity == null ? null : mapper.Map<UserDto>(entity);
        }

        public async Task<UserDto> CreateAsync(CreateUserDto dto)
        {
            // Проверяем уникальность email и username
            if (await IsEmailExistsAsync(dto.Email))
                throw new BusinessException($"Пользователь с email {dto.Email} уже существует");

            if (await IsUserNameExistsAsync(dto.UserName))
                throw new BusinessException($"Пользователь с именем {dto.UserName} уже существует");

            var entity = mapper.Map<UserEntity>(dto);
            await userRepository.AddAsync(entity);
            
            var createdEntity = await userRepository.GetByIdAsync(entity.Id);
            return mapper.Map<UserDto>(createdEntity!);
        }

        public async Task<UserDto?> UpdateAsync(Guid id, UpdateUserDto dto)
        {
            var entity = await userRepository.GetByIdAsync(id);
            if (entity == null)
                throw new NotFoundException($"Пользователь с ID {id} не найден");

            // Проверяем уникальность email если он изменился
            if (!string.IsNullOrEmpty(dto.Email) && dto.Email != entity.Email)
            {
                if (await IsEmailExistsAsync(dto.Email))
                    throw new BusinessException($"Пользователь с email {dto.Email} уже существует");
            }

            // Проверяем уникальность username если он изменился
            if (!string.IsNullOrEmpty(dto.UserName) && dto.UserName != entity.UserName)
            {
                if (await IsUserNameExistsAsync(dto.UserName))
                    throw new BusinessException($"Пользователь с именем {dto.UserName} уже существует");
            }

            await userRepository.UpdateAsync(id, dto.UserName, dto.Email, dto.PasswordHash);
            
            var updatedEntity = await userRepository.GetByIdAsync(id);
            return mapper.Map<UserDto>(updatedEntity!);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await userRepository.GetByIdAsync(id);
            if (entity == null)
                throw new NotFoundException($"Пользователь с ID {id} не найден");

            await userRepository.DeleteAsync(id);
            return true;
        }

        public async Task<List<TaskListItemDto>> GetUserTasksAsync(Guid userId)
        {
            var user = await userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new NotFoundException($"Пользователь с ID {userId} не найден");

            var tasks = await userRepository.GetUserTasksAsync(userId);
            return mapper.Map<List<TaskListItemDto>>(tasks);
        }

        public async Task<List<ProjectDto>> GetUserProjectsAsync(Guid userId)
        {
            var user = await userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new NotFoundException($"Пользователь с ID {userId} не найден");

            var projects = await userRepository.GetUserProjectsAsync(userId);
            return mapper.Map<List<ProjectDto>>(projects);
        }

        public async Task<List<TaskListItemDto>> GetUserCreatedTasksAsync(Guid userId)
        {
            var user = await userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new NotFoundException($"Пользователь с ID {userId} не найден");

            var tasks = await userRepository.GetUserCreatedTasksAsync(userId);
            return mapper.Map<List<TaskListItemDto>>(tasks);
        }

        public async Task<List<TaskListItemDto>> GetUserAssignedTasksAsync(Guid userId)
        {
            var user = await userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new NotFoundException($"Пользователь с ID {userId} не найден");

            var tasks = await userRepository.GetUserAssignedTasksAsync(userId);
            return mapper.Map<List<TaskListItemDto>>(tasks);
        }

        public async Task<bool> IsUserExistsAsync(Guid userId)
        {
            var user = await userRepository.GetByIdAsync(userId);
            return user != null;
        }

        public async Task<bool> IsEmailExistsAsync(string email)
        {
            return await userRepository.IsEmailExistsAsync(email);
        }

        public async Task<bool> IsUserNameExistsAsync(string userName)
        {
            return await userRepository.IsUserNameExistsAsync(userName);
        }
    }
}

