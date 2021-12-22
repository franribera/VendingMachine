using Api.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Respawn;

namespace Api.UnitTests.Fixtures;

public class TestFixture
{
    private static readonly Checkpoint Checkpoint = new();

    public VendingMachineDbContext DbContext => VendingMachineDbContextFactory.Create();

    public TestFixture()
    {
        Checkpoint.TablesToIgnore = new[] { VendingMachineDbContext.MigrationsTableName, nameof(VendingMachineDbContext.Roles) };

        DbContext.Database.Migrate();
    }

    public static void ResetDatabase()
    {
        Checkpoint.Reset(ConfigurationProvider.DatabaseConnectionString).GetAwaiter().GetResult();
    }
}