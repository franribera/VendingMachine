using Api.Features.Users.Read;
using Api.IntegrationTests.Fixtures;
using FluentAssertions;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Api.IntegrationTests.Features.Users;

[Collection(nameof(TestFixture)), ResetDatabase]
public class ReadUserTests
{
    private readonly HttpClient _httpClient;

    public ReadUserTests(TestFixture testFixture)
    {
        _httpClient = testFixture.BuildHttpClient();
    }

    [Fact]
    public async Task User_cannot_be_read_without_authentication()
    {
        // Act
        var response = await _httpClient.GetAsync("/user", CancellationToken.None);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task User_can_read_itself()
    {
        // Arrange
        var defaultUser = await _httpClient.AuthenticateDefaultUser();

        // Act
        var response = await _httpClient.GetAsync("/user", CancellationToken.None);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var readUser = await response.Content.ReadFromJsonAsync<ReadUserResponse>();
        readUser.Id.Should().Be(defaultUser.Id);
        readUser.Username.Should().Be(defaultUser.Username);
        readUser.Role.Should().Be(defaultUser.Role.Name);
    }
}