using System;
using System.Threading;
using System.Threading.Tasks;
using Api.Domain.Entities;
using Api.Domain.Enumerations;
using Api.Features.Products.Create;
using Api.UnitTests.Fixtures;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Api.UnitTests.Features.Products.Create;

[Collection(nameof(TestFixture)), ResetDatabase]
public class CreateProductRequestTests
{
    private readonly CreateProductRequestHandler _handler;

    public CreateProductRequestTests(TestFixture testFixture)
    {
        _handler = new CreateProductRequestHandler(testFixture.DbContext);
    }

    [Fact]
    public async Task Creates_the_product()
    {
        // Arrange
        var user = new User("Username", "Password", Role.Seller.Name);

        await using (var writeContext = VendingMachineDbContextFactory.Create())
        {
            await writeContext.Users.AddAsync(user);
            await writeContext.SaveChangesAsync();
        }

        var request = new CreateProductRequest
        {
            Name = "Product",
            Price = 100,
            Quantity = 10,
            UserId = user.Id
        };

        // Act
        var response = await _handler.Handle(request, CancellationToken.None);

        // Assert
        await using var readContext = VendingMachineDbContextFactory.Create();
        var storedProduct = await readContext.Products.SingleAsync();
        storedProduct.Should().NotBeNull();

        response.Id.Should().Be(storedProduct.Id);
        response.Name.Should().Be(storedProduct.Name);
        response.Price.Should().Be(storedProduct.Price.Amount);
        response.Quantity.Should().Be(storedProduct.Quantity);
    }

    [Fact]
    public async Task Throws_if_the_product_already_exists()
    {
        // Arrange
        var user = new User("Username", "Password", Role.Seller.Name);
        

        await using (var writeContext = VendingMachineDbContextFactory.Create())
        {
            await writeContext.Users.AddAsync(user);
            await writeContext.SaveChangesAsync();

            var product = new Product("Product", user.Id, 1, 10);
            await writeContext.Products.AddAsync(product);
            await writeContext.SaveChangesAsync();
        }

        var request = new CreateProductRequest
        {
            Name = "Product",
            Price = 100,
            Quantity = 10,
            UserId = user.Id
        };

        Func<Task<CreateProductResponse>> createFunc = async () => await _handler.Handle(request, CancellationToken.None);

        // Act - Assert
        await createFunc.Should().ThrowAsync<InvalidOperationException>();
    }
}