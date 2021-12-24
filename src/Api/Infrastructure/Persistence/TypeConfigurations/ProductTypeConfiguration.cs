using Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api.Infrastructure.Persistence.TypeConfigurations;

public class ProductTypeConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).ValueGeneratedOnAdd();

        builder
            .HasOne<User>()
            .WithOne()
            .HasForeignKey<Product>(s => s.SellerId)
            .HasPrincipalKey<User>(p => p.Id)
            .OnDelete(DeleteBehavior.Cascade);
    }
}