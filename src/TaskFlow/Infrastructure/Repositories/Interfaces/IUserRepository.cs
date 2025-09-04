using TaskFlow.Data.Entities;

namespace TaskFlow.Data.Repositories.Interfaces;

public interface IUserRepository
{
    Task<List<UserEntity>> GetAsync();
    Task<UserEntity?> GetByIdAsync(Guid id);
    Task<List<UserEntity>> GetWithProjectsAsync();
    Task AddAsync(Guid id, string userName, string passwordHash, string fullName);
    Task UpdateUserNameAsync(Guid id, string userName);
    Task UpdatePasswordAsync(Guid id, string passwordHash);
    Task DeleteAsync(Guid id);
}