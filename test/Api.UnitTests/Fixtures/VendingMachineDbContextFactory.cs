using Api.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Api.UnitTests.Fixtures;

public static class VendingMachineDbContextFactory
{
    public static VendingMachineDbContext Create()
    {
        var options = new DbContextOptionsBuilder<VendingMachineDbContext>()
            .UseSqlite(ConfigurationProvider.DatabaseConnectionString, builder =>
            {
                builder.MigrationsAssembly(typeof(VendingMachineDbContext).Assembly.FullName);
            })
            .Options;

        return new VendingMachineDbContext(options);
    }
}