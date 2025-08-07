using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskFlow.Data.Entities;

namespace TaskFlow.Data.Configurations;

public class TaskConfiguration : IEntityTypeConfiguration<TaskEntity>
{
    public void Configure(EntityTypeBuilder<TaskEntity> builder)
    {
        builder.HasKey(k => k.Id);

        builder
            .HasOne(p => p.Project)
            .WithMany(t => t.Tasks)
            .HasForeignKey(t => t.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(u => u.Creator)
            .WithMany(t => t.CreatedTasks)
            .HasForeignKey(t => t.CreatorId)
            .OnDelete(DeleteBehavior.Restrict); 

        builder
            .HasOne(u => u.Assignee)
            .WithMany(t => t.AssignedTasks)
            .HasForeignKey(t => t.AssigneeId)
            .OnDelete(DeleteBehavior.SetNull); 

        // Оптимизация (Что именно?)
        builder.HasIndex(t => t.CreatorId);
        builder.HasIndex(t => t.AssigneeId);
        builder.HasIndex(t => t.ProjectId);
        builder.HasIndex(t => t.Status);
        builder.HasIndex(t => t.Priority);
    }
}