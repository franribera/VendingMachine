using System;
using System.Net.Http;
using Api.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Respawn;

namespace Api.IntegrationTests.Fixtures;

public class TestFixture
{
    private static readonly Checkpoint Checkpoint = new();
    private readonly IHttpClientFactory _httpClientFactory;

    public VendingMachineDbContext DbContext => VendingMachineDbContextFactory.Create();

    public TestFixture()
    {
        Checkpoint.TablesToIgnore = new[] { VendingMachineDbContext.MigrationsTableName, nameof(VendingMachineDbContext.Roles) };

        DbContext.Database.Migrate();

        var services = new ServiceCollection().AddHttpClient().BuildServiceProvider();
        _httpClientFactory = services.GetRequiredService<IHttpClientFactory>();
    }

    public static void ResetDatabase()
    {
        Checkpoint.Reset(ConfigurationProvider.DatabaseConnectionString).GetAwaiter().GetResult();
    }

    public HttpClient BuildHttpClient()
    {
        var httpClient = _httpClientFactory.CreateClient();
        httpClient.BaseAddress = new Uri(ConfigurationProvider.ApiBaseAddress);

        return httpClient;
    }
}