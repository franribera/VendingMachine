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

        builder.OwnsOne(u => u.Password, password =>
        {
            password.Property(un => un.Value).HasColumnName("Password");
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