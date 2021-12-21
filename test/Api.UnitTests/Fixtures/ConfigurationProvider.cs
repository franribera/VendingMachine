using Microsoft.Extensions.Configuration;

namespace Api.UnitTests.Fixtures;

public static class ConfigurationProvider
{
    private static IConfiguration Configuration => new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", true)
        .AddEnvironmentVariables()
        .Build();

    public static string DatabaseConnectionString => Configuration.GetConnectionString("VendingDatabase");
}