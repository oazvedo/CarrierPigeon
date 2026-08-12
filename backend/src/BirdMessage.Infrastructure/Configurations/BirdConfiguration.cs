using BirdMessage.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BirdMessage.Infrastructure.Configurations;

public class BirdConfiguration : IEntityTypeConfiguration<Bird>
{
    public void Configure(EntityTypeBuilder<Bird> builder)
    {
        builder.ToTable("birds");

        builder.Property(b => b.Id).HasColumnName("id");
        builder.Property(b => b.Name).HasColumnName("name");
        builder.Property(b => b.Description).HasColumnName("description");
        builder.Property(b => b.CreatedAt).HasColumnName("created_at");
        builder.Property(b => b.UpdatedAt).HasColumnName("updated_at");
        builder.Property(b => b.Velocity).HasColumnName("velocity").HasPrecision(10, 2);
    }
}
