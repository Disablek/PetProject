using TaskFlow.Business.DTO.Project;
using TaskFlow.Business.DTO.Task;

namespace TaskFlow.Business.DTO.User;
public class UpdateUserDto
{
    public string UserName { get; set; } = string.Empty;
    public string? Email { get; set; }

    public List<ProjectDto> Projects { get; set; } = new();
    public List<TaskPreviewDto> CreatedTasks { get; set; } = new();
    public List<TaskPreviewDto> AssignedTasks { get; set; } = new();
}
