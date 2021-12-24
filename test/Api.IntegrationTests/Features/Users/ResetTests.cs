using Api.Domain.Entities;
using Api.Domain.Enumerations;
using Api.Features.Users.Reset;
using Api.IntegrationTests.Fixtures;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Api.Domain.ValueObjects;
using Xunit;

namespace Api.IntegrationTests.Features.Users;

[Collection(nameof(TestFixture)), ResetDatabase]
public class ResetTests
{
    private readonly HttpClient _httpClient;

    private class ResetResponse
    {
        public long UserId { get; set; }
        public int Amount { get; set; }
        public IEnumerable<int> Coins { get; set; } = new List<int>();
    }

    public ResetTests(TestFixture testFixture)
    {
        _httpClient = testFixture.BuildHttpClient();
    }

    [Fact]
    public async Task Buyer_cannot_reset_without_authentication()
    {
        // Arrange
        var request = new ResetRequest();

        // Act
        var response = await _httpClient.PostAsJsonAsync("/reset", request, CancellationToken.None);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Buyer_can_reset()
    {
        // Arrange
        var buyer = new User("Username", "Password", Role.Buyer.Name);

        buyer.DepositMoney(Coin.HundredCent);
        buyer.DepositMoney(Coin.HundredCent);
        buyer.DepositMoney(Coin.TwentyCent);
        buyer.DepositMoney(Coin.TenCent);
        buyer.DepositMoney(Coin.FiveCent);

        await using (var writeContext = VendingMachineDbContextFactory.Create())
        {
            await writeContext.Users.AddAsync(buyer);
            await writeContext.SaveChangesAsync();
        }

        await _httpClient.AuthenticateUser("Username", "Password");

        var request = new ResetRequest();

        // Act
        var response = await _httpClient.PostAsJsonAsync("/reset", request, CancellationToken.None);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var resetResponse = await response.Content.ReadFromJsonAsync<ResetResponse>();
        resetResponse.UserId.Should().Be(buyer.Id);
        resetResponse.Amount.Should().Be(235);
        resetResponse.Coins.Where(c => c == 100).Should().HaveCount(2);
        resetResponse.Coins.Where(c => c == 50).Should().HaveCount(0);
        resetResponse.Coins.Where(c => c == 20).Should().HaveCount(1);
        resetResponse.Coins.Where(c => c == 10).Should().HaveCount(1);
        resetResponse.Coins.Where(c => c == 5).Should().HaveCount(1);
    }

    [Fact]
    public async Task Seller_is_forbidden()
    {
        // Arrange
        var buyer = new User("Username", "Password", Role.Seller.Name);

        await using (var writeContext = VendingMachineDbContextFactory.Create())
        {
            await writeContext.Users.AddAsync(buyer);
            await writeContext.SaveChangesAsync();
        }

        await _httpClient.AuthenticateUser("Username", "Password");

        var request = new ResetRequest();

        // Act
        var response = await _httpClient.PostAsJsonAsync("/reset", request, CancellationToken.None);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}