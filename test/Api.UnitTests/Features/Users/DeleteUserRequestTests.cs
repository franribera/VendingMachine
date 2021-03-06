using System.Threading;
using System.Threading.Tasks;
using Api.Domain.Entities;
using Api.Domain.Enumerations;
using Api.Features.Users.Delete;
using Api.UnitTests.Fixtures;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Api.UnitTests.Features.Users;

[Collection(nameof(TestFixture))]
public class DeleteUserRequestTests
{
    private readonly IRequestHandler<DeleteUserRequest, Unit> _handler;

    public DeleteUserRequestTests(TestFixture testFixture)
    {
        _handler = new DeleteUserRequestHandler(testFixture.DbContext);
    }

    [Fact]
    public async Task Deletes_the_user()
    {
        // Arrange
        var user = new User("Username", "Password", Role.Buyer.Name);

        await using (var writeContext = VendingMachineDbContextFactory.Create())
        {
            await writeContext.Users.AddAsync(user);
            await writeContext.SaveChangesAsync();
        }

        var request = new DeleteUserRequest { UserId = user.Id };

        // Act
        await _handler.Handle(request, CancellationToken.None);

        // Assert
        await using var readContext = VendingMachineDbContextFactory.Create();
        var storedUser = await readContext.Users.SingleOrDefaultAsync();
        storedUser.Should().BeNull();
    }

    [Fact]
    public async Task Deletes_all_the_seller_stuff()
    {
        // Arrange
        User user;

        await using (var writeContext = VendingMachineDbContextFactory.Create())
        {
            user = new User("Username", "Password", Role.Seller.Name);

            await writeContext.Users.AddAsync(user);
            await writeContext.SaveChangesAsync();

            var product = new Product("Laptop", user.Id, 10, 100);

            await writeContext.Products.AddAsync(product);
            await writeContext.SaveChangesAsync();
        }

        var request = new DeleteUserRequest { UserId = user.Id };

        // Act
        await _handler.Handle(request, CancellationToken.None);

        // Assert
        await using var readContext = VendingMachineDbContextFactory.Create();
        var storedUser = await readContext.Users.SingleOrDefaultAsync();
        storedUser.Should().BeNull();
        var storedProduct = await readContext.Products.SingleOrDefaultAsync();
        storedProduct.Should().BeNull();
    }
}