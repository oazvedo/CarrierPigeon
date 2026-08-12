using BirdMessage.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BirdMessage.Infrastructure.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.Property(u => u.Id).HasColumnName("id");
        builder.Property(u => u.Name).HasColumnName("name");
        builder.Property(u => u.Email).HasColumnName("email");
        builder.Property(u => u.Password).HasColumnName("password");
        builder.Property(u => u.CreatedAt).HasColumnName("created_at");
        builder.Property(u => u.Role).HasColumnName("role");
    }
}
