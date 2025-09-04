using TaskFlow.Business.DTO.Task;
using TaskFlow.Business.DTO.User;

namespace TaskFlow.Business.DTO.Project;
public class ProjectDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<TaskDto> Tasks { get; set; } = new();
    public List<UserDto> Users { get; set; } = new();
    public Guid AdminId { get; set; }
}
