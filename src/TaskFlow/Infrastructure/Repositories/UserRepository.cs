using Microsoft.EntityFrameworkCore;
using TaskFlow.Data.Entities;

namespace TaskFlow.Data.Repositories;
public class UserRepository
{
    private readonly TaskFlowDbContext _dbContext;
    public UserRepository(TaskFlowDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<UserEntity>> Get() =>
        await _dbContext.Users.AsNoTracking().ToListAsync();

    public async Task<UserEntity?> GetById(Guid id) =>
        await _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);

    public async Task<List<UserEntity>> GetWithProjects() =>
        await _dbContext.Users
            .AsNoTracking()
            .Include(c => c.Projects)
            .ToListAsync();

    public async Task Add(Guid id, string userName, string passwordHash)
    {
        var userEntity = new UserEntity()
        {
            Id = id,
            UserName = userName,
            PasswordHash = passwordHash
        };

        await _dbContext.AddAsync(userEntity);
        await _dbContext.SaveChangesAsync();
    }
    public async Task UpdateUserName(Guid id, string userName)
    {
        await _dbContext.Users
            .Where(p => p.Id == id)
            .ExecuteUpdateAsync(s => s.
                SetProperty(c => c.UserName, userName));
    }
    public async Task Delete(Guid id)
    {
        await _dbContext.Users
            .Where(p => p.Id == id)
            .ExecuteDeleteAsync();
    }

}
