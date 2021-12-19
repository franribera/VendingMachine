using Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api.Infrastructure.Persistence.TypeConfigurations;

public class UserTypeConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id).ValueGeneratedOnAdd();

        builder.OwnsOne(u => u.Username, username =>
        {
            username.Property(un => un.Value).HasColumnName("Username");
        });

        builder.OwnsOne(u => u.Password, password =>
        {
            password.Property(un => un.Value).HasColumnName("Password");
        });
    }
}