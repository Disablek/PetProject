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
            .Select(t => new TaskEntity
            {
                Id = t.Id,
                ProjectId = t.ProjectId,
                CreatorId = t.CreatorId,
                AssigneeId = t.AssigneeId,
                Status = t.Status,
            })
            .FirstOrDefaultAsync(t => t.Id == id);


    public async Task<TaskEntity> AddAsync(TaskEntity entity)
    {
        var taskEntity = entity;

        await _dbContext.AddAsync(taskEntity);
        await _dbContext.SaveChangesAsync();
        return entity;
    }

    public async Task<TaskEntity?> GetTrackedByIdAsync(Guid id)
    {
        return await _dbContext.Tasks.FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<TaskEntity> UpdateAsync(TaskEntity entity)
    {
        var entry = _dbContext.Entry(entity);
        if (entry.State == EntityState.Detached)
        {
            _dbContext.Tasks.Update(entity);
        }

        await _dbContext.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var affected = await _dbContext.Tasks.Where(t => t.Id == id).ExecuteDeleteAsync();
        return affected > 0;
    }
}