using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskFlow.Data.Entities;

namespace TaskFlow.Data.Configurations;

public class ProjectConfiguration : IEntityTypeConfiguration<ProjectEntity>
{
    public void Configure(EntityTypeBuilder<ProjectEntity> builder)
    {
        builder.HasKey(k => k.Id);

        builder
            .HasMany(c => c.Tasks)
            .WithOne(l => l.Project)
            .HasForeignKey(l => l.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(u => u.Users)
            .WithMany(p => p.Projects)
            .UsingEntity(j => j.ToTable("UserProjects"));

        builder
            .HasOne(u => u.Admin)
            .WithMany()
            .HasForeignKey(p => p.AdminId)
            .OnDelete(DeleteBehavior.Restrict); 


        builder.HasIndex(p => p.AdminId);
    }
}

