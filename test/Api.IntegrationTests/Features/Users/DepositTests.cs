using Api.Domain.Entities;
using Api.Domain.Enumerations;
using Api.Features.Users.Deposit;
using Api.IntegrationTests.Fixtures;
using FluentAssertions;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Api.Domain.ValueObjects;
using Xunit;

namespace Api.IntegrationTests.Features.Users;

[Collection(nameof(TestFixture)), ResetDatabase]
public class DepositTests
{
    private readonly HttpClient _httpClient;

    public DepositTests(TestFixture testFixture)
    {
        _httpClient = testFixture.BuildHttpClient();
    }

    [Fact]
    public async Task Buyer_cannot_deposit_without_authentication()
    {
        // Arrange
        var request = new DepositRequest
        {
            Coin = 100
        };

        // Act
        var response = await _httpClient.PostAsJsonAsync("/deposit", request, CancellationToken.None);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Buyer_can_deposit()
    {
        // Arrange
        var buyer = new User("Username", "Password", Role.Buyer.Name);

        buyer.DepositMoney(Coin.FiftyCent);

        await using (var writeContext = VendingMachineDbContextFactory.Create())
        {
            await writeContext.Users.AddAsync(buyer);
            await writeContext.SaveChangesAsync();
        }

        await _httpClient.AuthenticateUser("Username", "Password");

        var request = new DepositRequest
        {
            Coin = 100
        };

        // Act
        var response = await _httpClient.PostAsJsonAsync("/deposit", request, CancellationToken.None);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var depositResponse = await response.Content.ReadFromJsonAsync<DepositResponse>();
        depositResponse.UserId.Should().Be(buyer.Id);
        depositResponse.Deposit.Should().Be(150);
    }

    [Fact]
    public async Task Seller_is_forbidden()
    {
        // Arrange
        var seller = new User("Username", "Password", Role.Seller.Name);

        await using (var writeContext = VendingMachineDbContextFactory.Create())
        {
            await writeContext.Users.AddAsync(seller);
            await writeContext.SaveChangesAsync();
        }

        await _httpClient.AuthenticateUser("Username", "Password");

        var request = new DepositRequest
        {
            Coin = 100
        };

        // Act
        var response = await _httpClient.PostAsJsonAsync("/deposit", request, CancellationToken.None);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}