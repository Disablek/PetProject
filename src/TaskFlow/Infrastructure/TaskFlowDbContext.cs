using Microsoft.EntityFrameworkCore;
using TaskFlow.Data.Configurations;
using TaskFlow.Data.Entities;

namespace TaskFlow.Data;

public class TaskFlowDbContext(DbContextOptions<TaskFlowDbContext> options) 
    : DbContext(options)
{
    public DbSet<ProjectEntity> Projects { get; set; }
    public DbSet<TaskEntity> Tasks { get; set; }
    public DbSet<UserEntity> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ProjectConfiguration());
        modelBuilder.ApplyConfiguration(new TaskConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}

