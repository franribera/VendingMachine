using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Api.Domain.Entities;
using Api.Domain.Enumerations;
using Api.Domain.ValueObjects;
using Api.Features.Products.Purchase;
using Api.IntegrationTests.Fixtures;
using FluentAssertions;
using Xunit;

namespace Api.IntegrationTests.Features.Products;

[Collection(nameof(TestFixture)), ResetDatabase]
public class BuyProductTests
{
    private readonly HttpClient _httpClient;

    public BuyProductTests(TestFixture testFixture)
    {
        _httpClient = testFixture.BuildHttpClient();
    }

    [Fact]
    public async Task Sellers_are_forbidden()
    {
        // Arrange
        var seller = new User("Username", "Password", Role.Seller.Name);

        await using (var writeContext = VendingMachineDbContextFactory.Create())
        {
            await writeContext.Users.AddAsync(seller);
            await writeContext.SaveChangesAsync();
        }

        await _httpClient.AuthenticateUser("Username", "Password");

        var request = new PurchaseProductRequest();

        // Act
        var response = await _httpClient.PostAsJsonAsync("/buy", request, CancellationToken.None);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Buyer_gets_conflict_when_not_enough_money()
    {
        // Arrange
        var buyer = new User("Buyer", "Password", Role.Buyer.Name);
        buyer.DepositMoney(Coin.FiftyCent);

        var seller = new User("Seller", "Password", Role.Seller.Name);

        await using (var writeContext = VendingMachineDbContextFactory.Create())
        {
            await writeContext.Users.AddAsync(seller);
            await writeContext.Users.AddAsync(buyer);
            await writeContext.SaveChangesAsync();
        }

        var product = new Product("Product", seller.Id, 1, Coin.HundredCent.Value);

        await using (var writeContext = VendingMachineDbContextFactory.Create())
        {
            await writeContext.Products.AddAsync(product);
            await writeContext.SaveChangesAsync();
        }

        await _httpClient.AuthenticateUser("Buyer", "Password");

        var request = new PurchaseProductRequest
        {
            ProductId = product.Id,
            Quantity = 1
        };

        // Act
        var response = await _httpClient.PostAsJsonAsync("/buy", request, CancellationToken.None);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task Buyer_gets_conflict_when_not_enough_stock()
    {
        // Arrange
        var buyer = new User("Buyer", "Password", Role.Buyer.Name);
        buyer.DepositMoney(Coin.FiftyCent);

        var seller = new User("Seller", "Password", Role.Seller.Name);

        await using (var writeContext = VendingMachineDbContextFactory.Create())
        {
            await writeContext.Users.AddAsync(seller);
            await writeContext.Users.AddAsync(buyer);
            await writeContext.SaveChangesAsync();
        }

        var product = new Product("Product", seller.Id, 1, Coin.TenCent.Value);

        await using (var writeContext = VendingMachineDbContextFactory.Create())
        {
            await writeContext.Products.AddAsync(product);
            await writeContext.SaveChangesAsync();
        }

        await _httpClient.AuthenticateUser("Buyer", "Password");

        var request = new PurchaseProductRequest
        {
            ProductId = product.Id,
            Quantity = 2
        };

        // Act
        var response = await _httpClient.PostAsJsonAsync("/buy", request, CancellationToken.None);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task Buyer_gets_purchase_summary_after_buying()
    {
        // Arrange
        var buyer = new User("Buyer", "Password", Role.Buyer.Name);
        buyer.DepositMoney(Coin.HundredCent);
        buyer.DepositMoney(Coin.HundredCent);
        buyer.DepositMoney(Coin.HundredCent);

        var seller = new User("Seller", "Password", Role.Seller.Name);

        await using (var writeContext = VendingMachineDbContextFactory.Create())
        {
            await writeContext.Users.AddAsync(seller);
            await writeContext.Users.AddAsync(buyer);
            await writeContext.SaveChangesAsync();
        }

        var product = new Product("Product", seller.Id, 1, 200);

        await using (var writeContext = VendingMachineDbContextFactory.Create())
        {
            await writeContext.Products.AddAsync(product);
            await writeContext.SaveChangesAsync();
        }

        await _httpClient.AuthenticateUser("Buyer", "Password");

        var request = new PurchaseProductRequest
        {
            ProductId = product.Id,
            Quantity = 1
        };

        // Act
        var response = await _httpClient.PostAsJsonAsync("/buy", request, CancellationToken.None);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var purchaseSummary = await response.Content.ReadFromJsonAsync<PurchaseProductResponse>();
        purchaseSummary.ProductId.Should().Be(product.Id);
        purchaseSummary.ProductName.Should().Be(product.Name);
        purchaseSummary.Quantity.Should().Be(request.Quantity);
        purchaseSummary.Cost.Should().Be(product.Price.Amount);
        purchaseSummary.Change.Should().Be(Coin.HundredCent.Value);
        purchaseSummary.Coins.Single().Should().Be(Coin.HundredCent.Value);
    }
}