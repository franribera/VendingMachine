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

        builder.HasIndex(p => p.Name).IsUnique();

        builder.OwnsOne(p => p.Price, price =>
        {
            price.Property(p => p.Amount).HasColumnName("Price");
        });

        builder
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(p => p.SellerId)
            .HasPrincipalKey(u => u.Id)
            .OnDelete(DeleteBehavior.Cascade);
    }
}