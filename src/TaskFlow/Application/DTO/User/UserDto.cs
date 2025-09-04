using TaskFlow.Business.DTO.Project;
using TaskFlow.Business.DTO.Task;

namespace TaskFlow.Business.DTO.User;
public class UserDto
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string FullName { get; set; }
    public string? Email { get; set; }

    public List<ProjectDto> Projects { get; set; } = new();
    public List<TaskDto> CreatedTasks { get; set; } = new();
    public List<TaskDto> AssignedTasks { get; set; } = new();
}
