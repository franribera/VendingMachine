using System;
using System.Threading;
using System.Threading.Tasks;
using Api.Domain.Entities;
using Api.Domain.Enumerations;
using Api.Features.Users.Create;
using Api.UnitTests.Fixtures;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Api.UnitTests.Features.Users;

[Collection(nameof(TestFixture)), ResetDatabase]
public class CreateUserRequestTests
{
    private readonly CreateUserRequestHandler _handler;

    public CreateUserRequestTests(TestFixture testFixture)
    {
        _handler = new CreateUserRequestHandler(testFixture.DbContext);
    }

    [Fact]
    public async Task Creates_the_user()
    {
        // Arrange
        var request = new CreateUserRequest
        {
            Password = "SuperStrongPassword",
            Username = "Whatever",
            Role = Role.Buyer.Name
        };

        // Act
        var response = await _handler.Handle(request, CancellationToken.None);

        // Assert
        await using var readContext = VendingMachineDbContextFactory.Create();
        var user = await readContext.Users.SingleOrDefaultAsync();
        user.Should().NotBeNull();
        user.Id.Should().Be(response.Id);
        user.Username.Value.Should().Be(response.Username);
        user.Role.Name.Should().Be(response.Role);
    }

    [Fact]
    public async Task Throws_when_user_already_exists()
    {
        // Arrange
        var user = new User("Username", "Password", Role.Seller.Name);

        await using (var writeContext = VendingMachineDbContextFactory.Create())
        {
            await writeContext.Users.AddAsync(user);
            await writeContext.SaveChangesAsync();
        }
        
        var request = new CreateUserRequest
        {
            Username = user.Username.Value,
            Password = "SuperStrongPassword",
            Role = Role.Buyer.Name
        };

        Func<Task<CreateUserResponse>> createAction = async () => await _handler.Handle(request, CancellationToken.None);

        // Act - Assert
        await createAction.Should().ThrowAsync<InvalidOperationException>();
    }
}