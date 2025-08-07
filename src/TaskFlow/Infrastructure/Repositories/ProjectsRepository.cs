using Microsoft.EntityFrameworkCore;
using TaskFlow.Data.Entities;

namespace TaskFlow.Data.Repositories;

public class ProjectsRepository
{
    private readonly TaskFlowDbContext _dbContext;
    public ProjectsRepository(TaskFlowDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<ProjectEntity>> Get() => 
        await _dbContext.Projects.AsNoTracking().ToListAsync();

    public async Task<ProjectEntity?> GetById(Guid id) =>
        await _dbContext.Projects
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);

    public async Task<List<ProjectEntity>> GetWithTask() =>
        await _dbContext.Projects
            .AsNoTracking()
            .Include(c => c.Tasks)
            .ToListAsync();

    public async Task<List<ProjectEntity>> GetByPage(int page, int pageSize) //Пагинация
    {
        return await _dbContext.Projects
            .AsNoTracking()
            .Skip((page -1) *pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task Add(Guid id, Guid adminId, string name, string description)
    {
        var projectEntity = new ProjectEntity
        {
            Id = id,
            AdminId = adminId,
            Name = name,
            Description = description
        };

        await _dbContext.AddAsync(projectEntity);
        await _dbContext.SaveChangesAsync();
    }
    public async Task Update(Guid id, Guid adminId, string name, string description)
    {
        await _dbContext.Projects
            .Where(p => p.Id == id)
            .ExecuteUpdateAsync(s => s.
                SetProperty(c => c.Name, name)
                .SetProperty(c=> c.Description, description));
    }
    public async Task Delete(Guid id)
    {
        await _dbContext.Projects
            .Where(p => p.Id == id)
            .ExecuteDeleteAsync();
    }


}

