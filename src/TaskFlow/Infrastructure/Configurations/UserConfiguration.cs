using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskFlow.Data.Entities;

namespace TaskFlow.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.HasKey(k => k.Id);

        builder
            .HasMany(p => p.Projects)
            .WithMany(u => u.Users);
            //.UsingEntity(j => j.ToTable("UserProjects"));


        builder.HasIndex(u => u.UserName).IsUnique();
    }
}


