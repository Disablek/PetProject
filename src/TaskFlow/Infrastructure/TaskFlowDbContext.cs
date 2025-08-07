using Microsoft.EntityFrameworkCore;
using TaskFlow.Data.Configurations;
using TaskFlow.Data.Entities;
using TaskFlow.Data.Entities.Interfaces;

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
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e => e.Entity is IAuditable &&
                        (e.State == EntityState.Added || e.State == EntityState.Modified));

        var now = DateTime.UtcNow;

        foreach (var entry in entries)
        {
            var auditable = (IAuditable)entry.Entity;

            if (entry.State == EntityState.Added)
                auditable.CreatedAt = now;

            auditable.UpdatedAt = now;
        }

        var deletedEntries = ChangeTracker
            .Entries()
            .Where(e => e.Entity is ISoftDeletable && e.State == EntityState.Deleted);

        foreach (var entry in deletedEntries)
        {
            // мягкое удаление
            entry.State = EntityState.Modified;
            ((ISoftDeletable)entry.Entity).DeletedAt = now;
        }

        return await base.SaveChangesAsync(cancellationToken);
    }

}

