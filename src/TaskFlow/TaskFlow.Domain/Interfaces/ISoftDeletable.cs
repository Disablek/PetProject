namespace TaskFlow.Domain.Entities.Interfaces;
public interface ISoftDeletable
{
    DateTime? DeletedAt { get; set; }
}
