using System.Reflection;
using Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Api.Infrastructure.Persistence;

public class VendingMachineDbContext : DbContext
{
    // dotnet ef migrations add Initial -p src/Api -s src/Api -o Infrastructure/Persistence/Migrations
    // dotnet ef database update -p src/Api -s src/Api

    public DbSet<User> Users { get; set; }

    public VendingMachineDbContext(DbContextOptions<VendingMachineDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
    }
}