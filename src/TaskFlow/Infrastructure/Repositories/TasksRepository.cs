using Microsoft.EntityFrameworkCore;
using TaskFlow.Data.Entities;
using TaskFlow.Data.Repositories.Interfaces;
using TaskFlow.Domain.Entities.Enums;

namespace TaskFlow.Data.Repositories;

public class TasksRepository : ITasksRepository
{
    private readonly TaskFlowDbContext _dbContext;
    public TasksRepository(TaskFlowDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<TaskEntity>> GetAllTasksAsync() =>
        await _dbContext.Tasks
            .AsNoTracking()
            .ToListAsync();
    

    public async Task<List<TaskEntity>> GetByProjectAsync(Guid projectId) =>
        await _dbContext.Tasks
            .AsNoTracking()
            .Where(t => t.ProjectId == projectId)
            .ToListAsync();

    public async Task<TaskEntity?> GetByIdAsync(Guid id) =>
        await _dbContext.Tasks
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);


    public async Task AddAsync(Guid id, Guid projectId,string title, string description, DateTime? dueTime, Priority priority, Guid creatorId)
    {
        var taskEntity = new TaskEntity
        {
            Id = id,
            ProjectId = projectId,
            Title = title,
            Description = description,
            DueTime = dueTime,
            Priority = priority,
            CreatorId = creatorId
        };

        await _dbContext.AddAsync(taskEntity);
        await _dbContext.SaveChangesAsync();
    }
    public async Task UpdateAsync(Guid id, string title, string description, DateTime? dueTime, Priority priority, Guid asigneeId)
    {
        await _dbContext.Tasks
            .Where(p => p.Id == id)
            .ExecuteUpdateAsync(s => s
                .SetProperty(c => c.Title, title)
                .SetProperty(c => c.Description, description)
                .SetProperty(c => c.DueTime, dueTime)
                .SetProperty(c => c.AssigneeId, asigneeId)
                .SetProperty(c => c.Priority, priority));
    }

    public async Task DeleteAsync(Guid id)
    {
        var task = await _dbContext.Tasks.FirstOrDefaultAsync(p => p.Id == id);
        if (task is not null)
        {
            _dbContext.Tasks.Remove(task);
            await _dbContext.SaveChangesAsync();
        }
    }

}