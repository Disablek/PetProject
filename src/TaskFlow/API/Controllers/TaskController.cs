using Microsoft.AspNetCore.Mvc;
using TaskFlow.Business.DTO;
using TaskFlow.Business.Services;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly TaskService _taskService;

        public TaskController(TaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<TaskItemListDto>> GetAllTasks()
        {
            var tasks = _taskService.GetAllTasks();
            return Ok(tasks);
        }

        [HttpGet("{id}")]
        public ActionResult<TaskItemDto> GetTaskById(Guid id)
        {
            var task = _taskService.GetTaskById(id);
            if (task == null)
                return NotFound();

            return Ok(task);
        }

        [HttpPost]
        public ActionResult<TaskItemDto> CreateTask(CreateTaskItemDto createDto)
        {
            if (string.IsNullOrWhiteSpace(createDto.Title))
                return BadRequest("Title is required");

            var task = _taskService.CreateTask(createDto);
            return CreatedAtAction(nameof(GetTaskById), new { id = task.Id }, task);
        }

        [HttpPut("{id}")]
        public ActionResult<TaskItemDto> UpdateTask(Guid id, UpdateTaskItemDto updateDto)
        {
            if (string.IsNullOrWhiteSpace(updateDto.Title))
                return BadRequest("Title is required");

            var task = _taskService.UpdateTask(id, updateDto);
            if (task == null)
                return NotFound();

            return Ok(task);
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteTask(Guid id)
        {
            var success = _taskService.DeleteTask(id);
            if (!success)
                return NotFound();

            return NoContent();
        }

        [HttpPatch("{id}/complete")]
        public ActionResult<TaskItemDto> MarkTaskCompleted(Guid id)
        {
            var task = _taskService.MarkTaskCompleted(id);
            if (task == null)
                return NotFound();

            return Ok(task);
        }
    }
} 