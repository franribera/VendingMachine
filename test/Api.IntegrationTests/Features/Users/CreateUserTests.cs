using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Api.Domain.Enumerations;
using Api.Features.Users.Create;
using Api.IntegrationTests.Fixtures;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Api.IntegrationTests.Features.Users;

[Collection(nameof(TestFixture)), ResetDatabase]
public class CreateUserTests
{
    private readonly HttpClient _httpClient;

    public CreateUserTests(TestFixture testFixture)
    {
        _httpClient = testFixture.BuildHttpClient();
    }

    [Fact]
    public async Task User_can_be_created_without_authentication()
    {
        // Arrange
        var request = new CreateUserRequest
        {
            Username = "RandomName",
            Password = "RandomPassword",
            Role = Role.Seller.Name
        };

        // Act
        var response = await _httpClient.PostAsJsonAsync("/user", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        
        var createdUser = await response.Content.ReadFromJsonAsync<CreateUserResponse>();
        createdUser.Id.Should().NotBe(default);
        createdUser.Username.Should().Be(request.Username);
        createdUser.Role.Should().Be(request.Role);
    }
}