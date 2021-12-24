using System.Collections.Generic;
using System.Linq;
using Api.IntegrationTests.Fixtures;
using FluentAssertions;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Api.Domain.Entities;
using Api.Domain.Enumerations;
using Api.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Api.IntegrationTests.Features.Users;

[Collection(nameof(TestFixture)), ResetDatabase]
public class DeleteUserTests
{
    private readonly HttpClient _httpClient;

    private class ResetResponse
    {
        public long UserId { get; set; }
        public int Amount { get; set; }
        public IEnumerable<int> Coins { get; set; } = new List<int>();
    }

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

    [Fact]
    public async Task Buyer_user_gets_back_the_remaining_money()
    {
        // Arrange
        var buyer = new User("Username", "Password", Role.Buyer.Name);
        buyer.DepositMoney(Coin.TwentyCent);

        await using (var writeContext = VendingMachineDbContextFactory.Create())
        {
            await writeContext.Users.AddAsync(buyer);
            await writeContext.SaveChangesAsync();
        }

        await _httpClient.AuthenticateUser("Username", "Password");

        // Act
        var response = await _httpClient.DeleteAsync("/user", CancellationToken.None);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var resetResponse = await response.Content.ReadFromJsonAsync<ResetResponse>();
        resetResponse.UserId.Should().Be(buyer.Id);
        resetResponse.Amount.Should().Be(20);
        resetResponse.Coins.Where(c => c == 100).Should().HaveCount(0);
        resetResponse.Coins.Where(c => c == 50).Should().HaveCount(0);
        resetResponse.Coins.Where(c => c == 20).Should().HaveCount(1);
        resetResponse.Coins.Where(c => c == 10).Should().HaveCount(0);
        resetResponse.Coins.Where(c => c == 5).Should().HaveCount(0);
    }
}