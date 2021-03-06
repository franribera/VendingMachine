using Api.Domain.Entities;
using Api.Domain.Enumerations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api.Infrastructure.Persistence.TypeConfigurations;

public class UserTypeConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id).ValueGeneratedOnAdd();

        builder.HasIndex(u => u.Username).IsUnique();
        builder.Ignore(u => u.Role);

        builder.OwnsOne(u => u.Password, password =>
        {
            password.Property(un => un.Value).HasColumnName("Password");
        });

        builder.OwnsOne(u => u.Deposit, deposit =>
        {
            deposit.Property(d => d.Amount).HasColumnName("Deposit");
        });

        builder
            .Property<int>("_roleId")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("RoleId")
            .IsRequired();

        builder
            .HasOne<Role>()
            .WithMany()
            .HasForeignKey("_roleId");
    }
}