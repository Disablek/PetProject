namespace TaskFlow.Business.DTO.Project;
public class AssignUserToProjectDto
{
    public Guid ProjectId { get; set; }
    public List<Guid> UsersId { get; set; }
}
