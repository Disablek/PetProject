using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TaskFlow.Data.Configurations;
using TaskFlow.Data.Entities;
using TaskFlow.Domain.Entities.Interfaces;

namespace TaskFlow.Data;

public class TaskFlowDbContext(DbContextOptions<TaskFlowDbContext> options) 
    : DbContext(options)
{
    public DbSet<ProjectEntity> Projects { get; set; }
    public DbSet<TaskEntity> Tasks { get; set; }
    public DbSet<UserEntity> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(ISAuditable).IsAssignableFrom(entityType.ClrType))
            {
                var parameter = Expression.Parameter(entityType.ClrType, "e");
                var property = Expression.Property(parameter, nameof(ISAuditable.DeletedAt));
                var condition = Expression.Equal(property, Expression.Constant(null));
                var lambda = Expression.Lambda(condition, parameter);

                entityType.SetQueryFilter(lambda);
            }
        }

        modelBuilder.ApplyConfiguration(new ProjectConfiguration());
        modelBuilder.ApplyConfiguration(new TaskConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());

        base.OnModelCreating(modelBuilder);
    }
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e => e.Entity is ISAuditable &&
                        (e.State == EntityState.Added || e.State == EntityState.Modified));

        var now = DateTime.UtcNow;

        foreach (var entry in entries)
        {
            var auditable = (ISAuditable)entry.Entity;

            if (entry.State == EntityState.Added)
                auditable.CreatedAt = now;

            auditable.UpdatedAt = now;
        }

        var deletedEntries = ChangeTracker
            .Entries()
            .Where(e => e.Entity is ISAuditable && e.State == EntityState.Deleted);

        foreach (var entry in deletedEntries)
        {
            // мягкое удаление
            entry.State = EntityState.Modified;
            ((ISAuditable)entry.Entity).DeletedAt = now;
        }

        return await base.SaveChangesAsync(cancellationToken);
    }

}

