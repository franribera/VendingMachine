using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Api.Domain.ValueObjects;
using Api.Features.Users.Update;
using Api.IntegrationTests.Fixtures;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Api.IntegrationTests.Features.Users;

[Collection(nameof(TestFixture)), ResetDatabase]
public class UpdateUserTests
{
    private readonly HttpClient _httpClient;

    public UpdateUserTests(TestFixture testFixture)
    {
        _httpClient = testFixture.BuildHttpClient();
    }

    [Fact]
    public async Task User_cannot_be_updated_without_authentication()
    {
        // Arrange
        var request = new UpdateUserRequest
        {
            UserId = 999,
            Password = "whatever"
        };

        // Act
        var response = await _httpClient.PutAsJsonAsync("/user", request, CancellationToken.None);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task User_can_update_itself()
    {
        // Arrange
        var defaultUser = await _httpClient.AuthenticateDefaultUser();

        var request = new UpdateUserRequest
        {
            Password = "whatever"
        };

        // Act
        var response = await _httpClient.PutAsJsonAsync("/user", request, CancellationToken.None);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var readUser = await response.Content.ReadFromJsonAsync<UpdateUserResponse>();
        readUser.Id.Should().Be(defaultUser.Id);
        readUser.Username.Should().Be(defaultUser.Username);
        readUser.Role.Should().Be(defaultUser.Role.Name);
    }
}