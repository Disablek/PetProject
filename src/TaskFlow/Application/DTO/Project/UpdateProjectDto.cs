namespace TaskFlow.Business.DTO.Project;
public class UpdateProjectDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public List<Guid> UserIds { get; set; } = new(); 
    public Guid AdminId { get; set; }
}
