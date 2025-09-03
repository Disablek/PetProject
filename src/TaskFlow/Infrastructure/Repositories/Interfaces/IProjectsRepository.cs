using TaskFlow.Data.Entities;

namespace TaskFlow.Data.Repositories.Interfaces;
public interface IProjectsRepository
{
    Task<List<ProjectEntity>> GetAsync();
    Task<ProjectEntity?> GetByIdAsync(Guid id);
    Task<List<ProjectEntity>> GetWithTaskAsync();
    Task<List<ProjectEntity>> GetByPageAsync(int page, int pageSize);
    Task AddAsync(Guid id, Guid adminId, string name, string description);
    Task UpdateAsync(Guid id, Guid adminId, string name, string description);
    Task DeleteAsync(Guid id);
}

