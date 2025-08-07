using TaskFlow.Data.Entities;

namespace TaskFlow.Data.Repositories.Interfaces;
public interface IProjectsRepository
{
    Task<List<ProjectEntity>> Get();
    Task<ProjectEntity?> GetById(Guid id);
    Task<List<ProjectEntity>> GetWithTask();
    Task<List<ProjectEntity>> GetByPage(int page, int pageSize);
    Task Add(Guid id, Guid adminId, string name, string description);
    Task Update(Guid id, Guid adminId, string name, string description);
    Task Delete(Guid id);
}

