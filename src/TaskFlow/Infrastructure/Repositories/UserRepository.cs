using Microsoft.EntityFrameworkCore;
using TaskFlow.Data.Entities;
using TaskFlow.Data.Repositories.Interfaces;

namespace TaskFlow.Data.Repositories;
public class UserRepository : IUserRepository
{
    private readonly TaskFlowDbContext _dbContext;
    public UserRepository(TaskFlowDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<UserEntity>> GetAsync() =>
        await _dbContext.Users.AsNoTracking().ToListAsync();

    public async Task<UserEntity?> GetByIdAsync(Guid id) =>
        await _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);

    public async Task<List<UserEntity>> GetWithProjectsAsync() =>
        await _dbContext.Users
            .AsNoTracking()
            .Include(c => c.Projects)
            .ToListAsync();

    public async Task AddAsync(UserEntity user)
    {
        await _dbContext.AddAsync(user);
        await _dbContext.SaveChangesAsync();
    }

    public async Task AddAsync(Guid id, string userName, string passwordHash, string fullName)
    {
        var userEntity = new UserEntity()
        {
            Id = id,
            UserName = userName,
            PasswordHash = passwordHash,
            FullName = fullName
        };

        await _dbContext.AddAsync(userEntity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Guid id, string? userName, string? email, string? passwordHash)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
        if (user != null)
        {
            if (!string.IsNullOrEmpty(userName))
                user.UserName = userName;
            if (!string.IsNullOrEmpty(email))
                user.Email = email;
            if (!string.IsNullOrEmpty(passwordHash))
                user.PasswordHash = passwordHash;
            
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task UpdateUserNameAsync(Guid id, string userName)
    {
        await _dbContext.Users
            .Where(p => p.Id == id)
            .ExecuteUpdateAsync(s => s.
                SetProperty(c => c.UserName, userName));
    }

    public async Task UpdatePasswordAsync(Guid id, string passwordHash)
    {
        await _dbContext.Users
            .Where(p => p.Id == id)
            .ExecuteUpdateAsync(s => s.
                SetProperty(c => c.PasswordHash, passwordHash));
    }

    public async Task DeleteAsync(Guid id)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(p => p.Id == id);
        if (user is not null)
        {
            _dbContext.Users.Remove(user); 
            await _dbContext.SaveChangesAsync(); 
        }
    }

    public async Task<List<TaskEntity>> GetUserTasksAsync(Guid userId)
    {
        return await _dbContext.Tasks
            .AsNoTracking()
            .Where(t => t.CreatorId == userId || t.AssigneeId == userId)
            .ToListAsync();
    }

    public async Task<List<ProjectEntity>> GetUserProjectsAsync(Guid userId)
    {
        return await _dbContext.Projects
            .AsNoTracking()
            .Where(p => p.Users.Any(u => u.Id == userId))
            .ToListAsync();
    }

    public async Task<List<TaskEntity>> GetUserCreatedTasksAsync(Guid userId)
    {
        return await _dbContext.Tasks
            .AsNoTracking()
            .Where(t => t.CreatorId == userId)
            .ToListAsync();
    }

    public async Task<List<TaskEntity>> GetUserAssignedTasksAsync(Guid userId)
    {
        return await _dbContext.Tasks
            .AsNoTracking()
            .Where(t => t.AssigneeId == userId)
            .ToListAsync();
    }

    public async Task<bool> IsEmailExistsAsync(string email)
    {
        return await _dbContext.Users
            .AnyAsync(u => u.Email == email);
    }

    public async Task<bool> IsUserNameExistsAsync(string userName)
    {
        return await _dbContext.Users
            .AnyAsync(u => u.UserName == userName);
    }
}
