namespace TaskFlow.Business.DTO.Project;
public class CreateProjectDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    // Можно сразу назначить участников по id (опционально)
    public List<Guid>? UserIds { get; set; }
    public Guid? AdminId { get; set; } 
}
