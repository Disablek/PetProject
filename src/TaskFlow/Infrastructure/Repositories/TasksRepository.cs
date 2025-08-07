using Microsoft.EntityFrameworkCore;
using TaskFlow.Data.Entities;
using TaskFlow.Data.Entities.Enums;
using TaskFlow.Data.Repositories.Interfaces;

namespace TaskFlow.Data.Repositories;

public class TasksRepository : ITasksRepository
{
    private readonly TaskFlowDbContext _dbContext;
    public TasksRepository(TaskFlowDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<TaskEntity>> GetByProject(Guid projectId) =>
        await _dbContext.Tasks
            .AsNoTracking()
            .Where(t => t.ProjectId == projectId)
            .ToListAsync();

    public async Task<TaskEntity?> GetById(Guid id) =>
        await _dbContext.Tasks
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);


    public async Task Add(Guid id, Guid projectId,string title, string description, DateTime? dueTime, Priority priority, Guid creatorId)
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
    public async Task Update(Guid id, string title, string description, DateTime? dueTime, Priority priority, Guid asigneeId)
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

    public async Task Delete(Guid id)
    {
        await _dbContext.Tasks
            .Where(p => p.Id == id)
            .ExecuteDeleteAsync();
    }

}