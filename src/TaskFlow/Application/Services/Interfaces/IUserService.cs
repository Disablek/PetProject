using TaskFlow.Business.DTO.User;
using TaskFlow.Business.DTO.Task;
using TaskFlow.Business.DTO.Project;

namespace TaskFlow.Business.Services.Interfaces
{
    public interface IUserService
    {
        Task<List<UserDto>> GetAllAsync();
        Task<UserDto?> GetByIdAsync(Guid id);
        Task<UserDto> CreateAsync(CreateUserDto dto);
        Task<UserDto?> UpdateAsync(Guid id, UpdateUserDto dto);
        Task<bool> DeleteAsync(Guid id);
        Task<List<TaskListItemDto>> GetUserTasksAsync(Guid userId);
        Task<List<ProjectDto>> GetUserProjectsAsync(Guid userId);
        Task<List<TaskListItemDto>> GetUserCreatedTasksAsync(Guid userId);
        Task<List<TaskListItemDto>> GetUserAssignedTasksAsync(Guid userId);
        Task<bool> IsUserExistsAsync(Guid userId);
        Task<bool> IsEmailExistsAsync(string email);
        Task<bool> IsUserNameExistsAsync(string userName);
    }
}

