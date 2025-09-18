using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TaskFlow.Business.DTO.Project;
using TaskFlow.Business.DTO.Task;
using TaskFlow.Business.DTO.User;
using TaskFlow.Business.Services.Interfaces;

namespace API.Controllers;

/// <summary>
/// Контроллер для управления проектами
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ProjectsController : ControllerBase
{
    private readonly IProjectService _projectService;

    public ProjectsController(IProjectService projectService)
    {
        _projectService = projectService;
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Получить все проекты", Tags = ["Projects"])]
    public async Task<List<ProjectDto>> GetAllProjects() => 
        await _projectService.GetAllAsync();

    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Получить проект по ID", Tags = ["Projects"])]
    public async Task<ProjectDto?> GetProjectById(Guid id) => 
        await _projectService.GetByIdAsync(id);

    [HttpPost]
    [SwaggerOperation(Summary = "Создать новый проект", Tags = ["Projects"])]
    public async Task<ProjectDto> CreateProject([FromBody] CreateProjectDto createProjectDto) => 
        await _projectService.CreateAsync(createProjectDto);

    [HttpPut("{id}")]
    [SwaggerOperation(Summary = "Обновить проект", Tags = ["Projects"])]
    public async Task<ProjectDto?> UpdateProject(Guid id, [FromBody] UpdateProjectDto updateProjectDto) => 
        await _projectService.UpdateAsync(id, updateProjectDto);

    [HttpDelete("{id}")]
    [SwaggerOperation(Summary = "Удалить проект", Tags = ["Projects"])]
    public async Task<IActionResult> DeleteProject(Guid id)
    {
        var result = await _projectService.DeleteAsync(id);
        return result ? NoContent() : NotFound();
    }

    [HttpGet("{id}/tasks")]
    [SwaggerOperation(Summary = "Получить задачи проекта", Tags = ["Projects"])]
    public async Task<List<TaskListItemDto>> GetProjectTasks(Guid id) => 
        await _projectService.GetProjectTasksAsync(id);

    [HttpGet("{id}/users")]
    [SwaggerOperation(Summary = "Получить пользователей проекта", Tags = ["Projects"])]
    public async Task<List<UserDto>> GetProjectUsers(Guid id) => 
        await _projectService.GetProjectUsersAsync(id);

    [HttpPost("{id}/users/{userId}")]
    [SwaggerOperation(Summary = "Добавить пользователя в проект", Tags = ["Projects"])]
    public async Task<ProjectDto> AddUserToProject(Guid id, Guid userId) => 
        await _projectService.AddUserToProjectAsync(id, userId);

    [HttpDelete("{id}/users/{userId}")]
    [SwaggerOperation(Summary = "Удалить пользователя из проекта", Tags = ["Projects"])]
    public async Task<ProjectDto> RemoveUserFromProject(Guid id, Guid userId) => 
        await _projectService.RemoveUserFromProjectAsync(id, userId);

    [HttpGet("{id}/is-admin/{userId}")]
    [SwaggerOperation(Summary = "Проверить является ли пользователь администратором проекта", Tags = ["Projects"])]
    public async Task<bool> IsUserAdmin(Guid id, Guid userId) => 
        await _projectService.IsUserAdminAsync(id, userId);

    [HttpGet("{id}/is-member/{userId}")]
    [SwaggerOperation(Summary = "Проверить является ли пользователь участником проекта", Tags = ["Projects"])]
    public async Task<bool> IsUserInProject(Guid id, Guid userId) => 
        await _projectService.IsUserInProjectAsync(id, userId);
}

