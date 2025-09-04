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

}
