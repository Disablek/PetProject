using Microsoft.EntityFrameworkCore;
using TaskFlow.Data.Entities;
using TaskFlow.Data.Repositories.Interfaces;

namespace TaskFlow.Data.Repositories;

public class ProjectsRepository : IProjectsRepository
{
    private readonly TaskFlowDbContext _dbContext;
    public ProjectsRepository(TaskFlowDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<ProjectEntity>> GetAsync() => 
        await _dbContext.Projects.AsNoTracking().ToListAsync();

    public async Task<ProjectEntity?> GetByIdAsync(Guid id) =>
        await _dbContext.Projects
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);

    public async Task<bool> IsUserAdminAsync(Guid projectId, Guid userId)
    {
        return await _dbContext.Projects
            .Where(p => p.Id == projectId && p.AdminId == userId)
            .AnyAsync();
    }

    public async Task<bool> IsUserInProjectAsync(Guid projectId, Guid userId)
    {
        return await _dbContext.Projects
            .Where(p => p.Id == projectId)
            .Select(p => p.Users.Any(u => u.Id == userId))
            .FirstOrDefaultAsync();
    }

    public async Task<List<ProjectEntity>> GetWithTaskAsync() =>
        await _dbContext.Projects
            .AsNoTracking()
            .Include(c => c.Tasks)
            .ToListAsync();

    public async Task<List<ProjectEntity>> GetByPageAsync(int page, int pageSize) //Пагинация
    {
        return await _dbContext.Projects
            .AsNoTracking()
            .Skip((page -1) *pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task AddAsync(Guid id, Guid adminId, string name, string description)
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
    public async Task UpdateAsync(Guid id, Guid adminId, string name, string description)
    {
        await _dbContext.Projects
            .Where(p => p.Id == id)
            .ExecuteUpdateAsync(s => s.
                SetProperty(c => c.Name, name)
                .SetProperty(c=> c.Description, description));
    }
    public async Task DeleteAsync(Guid id)
    {
        var project = await _dbContext.Projects.FirstOrDefaultAsync(p => p.Id == id);
        if (project is not null)
        {
            _dbContext.Projects.Remove(project); 
            await _dbContext.SaveChangesAsync(); 
        }

    }


}

