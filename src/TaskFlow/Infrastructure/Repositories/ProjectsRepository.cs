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

    public async Task<List<TaskEntity>> GetProjectTasksAsync(Guid projectId)
    {
        return await _dbContext.Tasks
            .AsNoTracking()
            .Where(t => t.ProjectId == projectId)
            .ToListAsync();
    }

    public async Task<List<UserEntity>> GetProjectUsersAsync(Guid projectId)
    {
        return await _dbContext.Projects
            .AsNoTracking()
            .Where(p => p.Id == projectId)
            .SelectMany(p => p.Users)
            .ToListAsync();
    }

    public async Task AddUserToProjectAsync(Guid projectId, Guid userId)
    {
        var project = await _dbContext.Projects
            .Include(p => p.Users)
            .FirstOrDefaultAsync(p => p.Id == projectId);
        
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
        
        if (project != null && user != null && !project.Users.Any(u => u.Id == userId))
        {
            project.Users.Add(user);
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task RemoveUserFromProjectAsync(Guid projectId, Guid userId)
    {
        var project = await _dbContext.Projects
            .Include(p => p.Users)
            .FirstOrDefaultAsync(p => p.Id == projectId);
        
        if (project != null)
        {
            var user = project.Users.FirstOrDefault(u => u.Id == userId);
            if (user != null)
            {
                project.Users.Remove(user);
                await _dbContext.SaveChangesAsync();
            }
        }
    }

    public async Task UpdateProjectUsersAsync(Guid projectId, List<Guid> userIds)
    {
        var project = await _dbContext.Projects
            .Include(p => p.Users)
            .FirstOrDefaultAsync(p => p.Id == projectId);
        
        if (project != null)
        {
            // Очищаем текущих пользователей
            project.Users.Clear();
            
            // Добавляем новых пользователей
            var users = await _dbContext.Users
                .Where(u => userIds.Contains(u.Id))
                .ToListAsync();
            
            foreach (var user in users)
            {
                project.Users.Add(user);
            }
            
            await _dbContext.SaveChangesAsync();
        }
    }


}

