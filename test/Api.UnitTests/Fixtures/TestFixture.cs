using System;
using Api.Infrastructure.Persistence;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Api.UnitTests.Fixtures;

public class TestFixture
{
    public VendingMachineDbContext DbContext => VendingMachineDbContextFactory.Create();

    public TestFixture()
    {
        //DatabaseConnection = new SqliteConnection(ConfigurationProvider.DatabaseConnectionString);

        //var dbContext = VendingMachineDbContextFactory.Create(DatabaseConnection);

        //DatabaseConnection.Open();

        //dbContext.Database.ExecuteSqlRaw("DROP TABLE IF EXISTS __EFMigrationsHistory;");
        //dbContext.Database.ExecuteSqlRaw("DROP TABLE IF EXISTS Users;");
        //dbContext.Database.ExecuteSqlRaw("DROP TABLE IF EXISTS Roles;");
        //dbContext.Database.EnsureCreated();
        //dbContext.Database.Migrate();

        RecreateDatabase();
    }

    public static void RecreateDatabase()
    {
        var dbContext = VendingMachineDbContextFactory.Create();

        dbContext.Database.OpenConnection();

        dbContext.Database.ExecuteSqlRaw("DROP TABLE IF EXISTS __EFMigrationsHistory;");
        dbContext.Database.ExecuteSqlRaw("DROP TABLE IF EXISTS Users;");
        dbContext.Database.ExecuteSqlRaw("DROP TABLE IF EXISTS Roles;");
        dbContext.Database.EnsureCreated();
        dbContext.Database.Migrate();

        dbContext.Database.CloseConnection();
    }

    public static void ResetDatabase()
    {
        var dbContext = VendingMachineDbContextFactory.Create();

        dbContext.Database.OpenConnection();

        dbContext.Database.ExecuteSqlRaw("DELETE FROM Users;");

        dbContext.Database.CloseConnection();
    }
}