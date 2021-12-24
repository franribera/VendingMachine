using Api.IntegrationTests.Fixtures;
using FluentAssertions;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Api.IntegrationTests.Features.Users;

[Collection(nameof(TestFixture)), ResetDatabase]
public class DeleteUserTests
{
    private readonly HttpClient _httpClient;

    public DeleteUserTests(TestFixture testFixture)
    {
        _httpClient = testFixture.BuildHttpClient();
    }

    [Fact]
    public async Task User_cannot_be_deleted_without_authentication()
    {
        // Act
        var response = await _httpClient.DeleteAsync("/user", CancellationToken.None);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task User_can_delete_it_self()
    {
        // Arrange
        var defaultUser = await _httpClient.AuthenticateDefaultUser();

        // Act
        var response = await _httpClient.DeleteAsync("/user", CancellationToken.None);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        await using var readContext = VendingMachineDbContextFactory.Create();
        var user = await readContext.Users.SingleOrDefaultAsync();
        user.Should().BeNull();
    }
}