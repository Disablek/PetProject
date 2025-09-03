namespace TaskFlow.Domain.Entities.Interfaces;
public interface IAuditable
{
    DateTime CreatedAt { get; set; }
    DateTime UpdatedAt { get; set; }
}
