using BirdMessage.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BirdMessage.Infrastructure.Configurations;

public class AddressConfiguration : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.ToTable("addresses");

        builder.Property(a => a.Id).HasColumnName("id");
        builder.Property(a => a.UserId).HasColumnName("user_id");
        builder.Property(a => a.Cep).HasColumnName("cep");
        builder.Property(a => a.Street).HasColumnName("street");
        builder.Property(a => a.Neighborhood).HasColumnName("neighborhood");
        builder.Property(a => a.Local).HasColumnName("local");
        builder.Property(a => a.Uf).HasColumnName("uf");
        builder.Property(a => a.State).HasColumnName("state");
        builder.Property(a => a.Region).HasColumnName("region");
        builder.Property(a => a.DDD).HasColumnName("ddd");
    }
}
