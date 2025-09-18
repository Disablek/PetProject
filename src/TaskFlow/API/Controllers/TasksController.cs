using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TaskFlow.Business.DTO.Task;
using TaskFlow.Business.Services.Interfaces;
using TaskFlow.Domain.Entities.Enums;

namespace API.Controllers;

/// <summary>
/// Контроллер для управления задачами
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class TasksController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TasksController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Получить все задачи", Tags = ["Tasks"])]
    public async Task<List<TaskListItemDto>> GetAllTasks() => 
        await _taskService.GetAllAsync();

    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Получить задачу по ID", Tags = ["Tasks"])]
    public async Task<TaskDto?> GetTaskById(Guid id) => 
        await _taskService.GetByIdAsync(id);

    [HttpGet("project/{projectId}")]
    [SwaggerOperation(Summary = "Получить задачи по проекту", Tags = ["Tasks"])]
    public async Task<List<TaskListItemDto>> GetTasksByProject(Guid projectId) => 
        await _taskService.GetByProjectAsync(projectId);

    [HttpPost]
    [SwaggerOperation(Summary = "Создать новую задачу", Tags = ["Tasks"])]
    public async Task<TaskDto> CreateTask([FromBody] CreateTaskDto createTaskDto) => 
        await _taskService.CreateAsync(createTaskDto);

    [HttpPut("{id}")]
    [SwaggerOperation(Summary = "Обновить задачу", Tags = ["Tasks"])]
    public async Task<TaskDto?> UpdateTask(Guid id, [FromBody] UpdateTaskDto updateTaskDto) => 
        await _taskService.UpdateAsync(id, updateTaskDto);

    [HttpDelete("{id}")]
    [SwaggerOperation(Summary = "Удалить задачу", Tags = ["Tasks"])]
    public async Task DeleteTask(Guid id, [FromQuery] Guid currentUserId) => 
        await _taskService.DeleteAsync(id, currentUserId);

    [HttpPatch("{id}/status")]
    [SwaggerOperation(Summary = "Изменить статус задачи", Tags = ["Tasks"])]
    public async Task<TaskDto?> ChangeTaskStatus(Guid id, [FromQuery] Status status, [FromQuery] Guid performedBy)
    {
        var currentTask = await _taskService.GetByIdAsync(id);
        return await _taskService.ChangeStatusAsync(id, status, performedBy, currentTask!);
    }

    [HttpPatch("{id}/assign")]
    [SwaggerOperation(Summary = "Назначить исполнителя задачи", Tags = ["Tasks"])]
    public async Task<TaskDto?> AssignTask(Guid id, [FromQuery] Guid assigneeId, [FromQuery] Guid changedBy) => 
        await _taskService.AssignAsync(id, assigneeId, changedBy);

    [HttpPatch("{id}/priority")]
    [SwaggerOperation(Summary = "Изменить приоритет задачи", Tags = ["Tasks"])]
    public async Task<TaskDto?> UpdateTaskPriority(Guid id, [FromQuery] Priority priority, [FromQuery] Guid currentUserId) => 
        await _taskService.UpdatePriorityAsync(id, priority, currentUserId);

    [HttpPatch("{id}/due-time")]
    [SwaggerOperation(Summary = "Изменить срок выполнения задачи", Tags = ["Tasks"])]
    public async Task<TaskDto?> UpdateTaskDueTime(Guid id, [FromQuery] DateTime? dueTime) => 
        await _taskService.UpdateDueTimeAsync(id, dueTime);
}
