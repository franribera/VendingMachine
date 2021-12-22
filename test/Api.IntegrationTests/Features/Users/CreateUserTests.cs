using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Api.Domain.Enumerations;
using Api.Features.Users.Create;
using Api.IntegrationTests.Fixtures;
using FluentAssertions;
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
        var response = await _httpClient.PostAsJsonAsync("/users", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
    }
}