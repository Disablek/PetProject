namespace TaskFlow.Data.Entities.Interfaces;
public interface ISoftDeletable
{
    DateTime? DeletedAt { get; set; }
}
