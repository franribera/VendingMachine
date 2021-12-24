using Api.Domain.Enumerations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api.Infrastructure.Persistence.TypeConfigurations;

public class RoleTypeConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).ValueGeneratedNever().IsRequired();

        builder.HasIndex(r => r.Name).IsUnique();

        builder.HasData(Enumeration.GetAll<Role>());
    }
}