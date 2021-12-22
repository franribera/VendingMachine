using Api.Domain.Entities;
using Api.Domain.Enumerations;
using Api.Features.Users.Create;
using Api.IntegrationTests.Fixtures;
using FluentAssertions;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
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
        var response = await _httpClient.DeleteAsync("/users/1", CancellationToken.None);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task User_can_delete_it_self()
    {
        // Arrange
        var defaultUser = await _httpClient.AuthenticateDefaultUser();

        // Act
        var response = await _httpClient.DeleteAsync($"/users/{defaultUser.Id}", CancellationToken.None);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Delete_other_user_is_forbidden()
    {
        // Arrange
        var otherUser = new User("RandomName", "RandomPassword", Role.Seller.Name);

        await using (var writeContext = VendingMachineDbContextFactory.Create())
        {
            await writeContext.Users.AddAsync(otherUser);
            await writeContext.SaveChangesAsync();
        }

        await _httpClient.AuthenticateDefaultUser();

        // Act
        var response = await _httpClient.DeleteAsync($"/users/{otherUser.Id}", CancellationToken.None);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}