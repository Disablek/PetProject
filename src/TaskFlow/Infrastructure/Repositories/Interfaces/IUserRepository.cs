using TaskFlow.Data.Entities;

namespace TaskFlow.Data.Repositories.Interfaces;

public interface IUserRepository
{
    Task<List<UserEntity>> Get();
    Task<UserEntity?> GetById(Guid id);
    Task<List<UserEntity>> GetWithProjects();
    Task Add(Guid id, string userName, string passwordHash);
    Task UpdateUserName(Guid id, string userName);
    Task Delete(Guid id);
}