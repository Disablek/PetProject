using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TaskFlow.Business.DTO.User;
using TaskFlow.Business.DTO.Task;
using TaskFlow.Business.DTO.Project;
using TaskFlow.Business.Services.Interfaces;

namespace API.Controllers;

/// <summary>
/// Контроллер для управления пользователями
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Получить всех пользователей", Tags = ["Users"])]
    public async Task<List<UserDto>> GetAllUsers() => 
        await _userService.GetAllAsync();

    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Получить пользователя по ID", Tags = ["Users"])]
    public async Task<UserDto?> GetUserById(Guid id) => 
        await _userService.GetByIdAsync(id);

    [HttpPost]
    [SwaggerOperation(Summary = "Создать нового пользователя", Tags = ["Users"])]
    public async Task<UserDto> CreateUser([FromBody] CreateUserDto createUserDto) => 
        await _userService.CreateAsync(createUserDto);

    [HttpPut("{id}")]
    [SwaggerOperation(Summary = "Обновить пользователя", Tags = ["Users"])]
    public async Task<UserDto?> UpdateUser(Guid id, [FromBody] UpdateUserDto updateUserDto) => 
        await _userService.UpdateAsync(id, updateUserDto);

    [HttpDelete("{id}")]
    [SwaggerOperation(Summary = "Удалить пользователя", Tags = ["Users"])]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        var result = await _userService.DeleteAsync(id);
        return result ? NoContent() : NotFound();
    }

    [HttpGet("{id}/tasks")]
    [SwaggerOperation(Summary = "Получить задачи пользователя (созданные + назначенные)", Tags = ["Users"])]
    public async Task<List<TaskListItemDto>> GetUserTasks(Guid id) => 
        await _userService.GetUserTasksAsync(id);

    [HttpGet("{id}/projects")]
    [SwaggerOperation(Summary = "Получить проекты пользователя", Tags = ["Users"])]
    public async Task<List<ProjectDto>> GetUserProjects(Guid id) => 
        await _userService.GetUserProjectsAsync(id);

    [HttpGet("{id}/created-tasks")]
    [SwaggerOperation(Summary = "Получить созданные пользователем задачи", Tags = ["Users"])]
    public async Task<List<TaskListItemDto>> GetUserCreatedTasks(Guid id) => 
        await _userService.GetUserCreatedTasksAsync(id);

    [HttpGet("{id}/assigned-tasks")]
    [SwaggerOperation(Summary = "Получить назначенные пользователю задачи", Tags = ["Users"])]
    public async Task<List<TaskListItemDto>> GetUserAssignedTasks(Guid id) => 
        await _userService.GetUserAssignedTasksAsync(id);

    [HttpGet("{id}/exists")]
    [SwaggerOperation(Summary = "Проверить существование пользователя", Tags = ["Users"])]
    public async Task<bool> IsUserExists(Guid id) => 
        await _userService.IsUserExistsAsync(id);

    [HttpGet("email/{email}/exists")]
    [SwaggerOperation(Summary = "Проверить существование email", Tags = ["Users"])]
    public async Task<bool> IsEmailExists(string email) => 
        await _userService.IsEmailExistsAsync(email);

    [HttpGet("username/{userName}/exists")]
    [SwaggerOperation(Summary = "Проверить существование имени пользователя", Tags = ["Users"])]
    public async Task<bool> IsUserNameExists(string userName) => 
        await _userService.IsUserNameExistsAsync(userName);
}

