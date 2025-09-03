namespace TaskFlow.Domain.Entities.Interfaces;
public class ISAuditable : ISSoftDeletable
{
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
