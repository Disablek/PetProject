using TaskFlow.Business.DTO.Project;
using TaskFlow.Business.DTO.Task;

namespace TaskFlow.Business.DTO.User;
public class CreateUserDto
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string FullName { get; set; }
    public string? Email { get; set; }
}
