namespace TaskFlow.Business.DTO.Project;
public class ProjcetListItemDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    /// <summary>
    /// Количество задач
    /// </summary>
    public int TasksCount { get; set; }   
    public Guid AdminId { get; set; }
}
