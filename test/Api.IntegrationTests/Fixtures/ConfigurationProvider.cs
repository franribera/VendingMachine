using System;
using Microsoft.Extensions.Configuration;

namespace Api.IntegrationTests.Fixtures;

public static class ConfigurationProvider
{
    private static IConfiguration Configuration => new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", true)
        .AddEnvironmentVariables()
        .Build();

    public static string DatabaseConnectionString => Configuration.GetConnectionString("VendingDatabase");
    public static string ApiBaseAddress => Configuration.GetSection("Api:BaseAddress").Get<string>();
}