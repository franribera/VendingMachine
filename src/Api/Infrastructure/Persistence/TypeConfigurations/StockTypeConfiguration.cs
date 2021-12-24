using Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api.Infrastructure.Persistence.TypeConfigurations;

public class StockTypeConfiguration : IEntityTypeConfiguration<Stock>
{
    public void Configure(EntityTypeBuilder<Stock> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).ValueGeneratedOnAdd();

        builder.OwnsOne(s => s.Price, deposit =>
        {
            deposit.Property(p => p.Amount).HasColumnName("Price");
        });

        builder
            .HasOne<Product>()
            .WithOne()
            .HasForeignKey<Stock>(s => s.ProductId)
            .HasPrincipalKey<Product>(p => p.Id)
            .OnDelete(DeleteBehavior.Cascade);
    }
}