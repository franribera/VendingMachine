using System;
using System.Threading;
using System.Threading.Tasks;
using Api.Domain.Entities;
using Api.Domain.Enumerations;
using Api.Domain.ValueObjects;
using Api.Features.Users.Update;
using Api.UnitTests.Fixtures;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Api.UnitTests.Features.Users;

[Collection(nameof(TestFixture)), ResetDatabase]
public class UpdateUserRequestTests
{
    private readonly UpdateUserRequestHandler _handler;

    public UpdateUserRequestTests(TestFixture testFixture)
    {
        _handler = new UpdateUserRequestHandler(testFixture.DbContext);
    }

    [Fact]
    public async Task Updates_the_username()
    {
        // Arrange
        var user = new User("Username", "Password", Role.Seller.Name);

        await using (var writeContext = VendingMachineDbContextFactory.Create())
        {
            await writeContext.Users.AddAsync(user);
            await writeContext.SaveChangesAsync();
        }

        var request = new UpdateUserRequest { UserId = user.Id, Username = "NewName" };

        // Act
        await _handler.Handle(request, CancellationToken.None);

        // Assert
        await using var readContext = VendingMachineDbContextFactory.Create();
        var storedUser = await readContext.Users.SingleOrDefaultAsync();
        storedUser.Should().NotBeNull();
        storedUser?.Id.Should().Be(user.Id);
        storedUser?.Username.Should().Be("NewName");
        storedUser?.Role.Should().Be(user.Role);
    }

    [Fact]
    public async Task Updates_the_password()
    {
        // Arrange
        var user = new User("Username", "Password", Role.Seller.Name);

        await using (var writeContext = VendingMachineDbContextFactory.Create())
        {
            await writeContext.Users.AddAsync(user);
            await writeContext.SaveChangesAsync();
        }

        var request = new UpdateUserRequest { UserId = user.Id, Password = "NewPassword" };

        // Act
        await _handler.Handle(request, CancellationToken.None);

        // Assert
        await using var readContext = VendingMachineDbContextFactory.Create();
        var storedUser = await readContext.Users.SingleOrDefaultAsync();
        storedUser.Should().NotBeNull();
        storedUser?.Id.Should().Be(user.Id);
        storedUser?.Username.Should().Be(user.Username);
        storedUser?.Password.Should().Be(new Password("NewPassword"));
        storedUser?.Role.Should().Be(user.Role);
    }

    [Fact]
    public async Task Updates_the_role()
    {
        // Arrange
        var user = new User("Username", "Password", Role.Seller.Name);

        await using (var writeContext = VendingMachineDbContextFactory.Create())
        {
            await writeContext.Users.AddAsync(user);
            await writeContext.SaveChangesAsync();
        }

        var request = new UpdateUserRequest { UserId = user.Id, Role = Role.Buyer.Name };

        // Act
        await _handler.Handle(request, CancellationToken.None);

        // Assert
        await using var readContext = VendingMachineDbContextFactory.Create();
        var storedUser = await readContext.Users.SingleOrDefaultAsync();
        storedUser.Should().NotBeNull();
        storedUser?.Id.Should().Be(user.Id);
        storedUser?.Username.Should().Be(user.Username);
        storedUser?.Password.Should().Be(user.Password);
        storedUser?.Role.Should().Be(Role.Buyer);
    }

    [Fact]
    public async Task Updates_the_whole_user()
    {
        // Arrange
        var user = new User("Username", "Password", Role.Seller.Name);

        await using (var writeContext = VendingMachineDbContextFactory.Create())
        {
            await writeContext.Users.AddAsync(user);
            await writeContext.SaveChangesAsync();
        }

        var request = new UpdateUserRequest
        {
            UserId = user.Id,
            Username = "NewName",
            Password = "NewPassword",
            Role = Role.Buyer.Name
        };

        // Act
        await _handler.Handle(request, CancellationToken.None);

        // Assert
        await using var readContext = VendingMachineDbContextFactory.Create();
        var storedUser = await readContext.Users.SingleOrDefaultAsync();
        storedUser.Should().NotBeNull();
        storedUser?.Id.Should().Be(user.Id);
        storedUser?.Username.Should().Be(request.Username);
        storedUser?.Password.Should().Be(new Password(request.Password));
        storedUser?.Role.Should().Be(Role.Buyer);
    }

    [Fact]
    public async Task Throws_when_username_already_exists()
    {
        // Arrange
        var user1 = new User("Username1", "Password", Role.Seller.Name);
        var user2 = new User("Username2", "Password", Role.Seller.Name);

        await using (var writeContext = VendingMachineDbContextFactory.Create())
        {
            await writeContext.Users.AddAsync(user1);
            await writeContext.Users.AddAsync(user2);
            await writeContext.SaveChangesAsync();
        }

        var request = new UpdateUserRequest { UserId = user1.Id, Username = user2.Username };

        Func<Task<UpdateUserResponse>> updateAction = async () => await _handler.Handle(request, CancellationToken.None);

        // Act - Assert
        await updateAction.Should().ThrowAsync<InvalidOperationException>();
    }
}