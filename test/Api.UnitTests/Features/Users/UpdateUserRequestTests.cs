using Api.Domain.Entities;
using Api.Domain.Enumerations;
using Api.Domain.ValueObjects;
using Api.Features.Users.Update;
using Api.UnitTests.Fixtures;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
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
    public async Task Updates_password()
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
            Password = "NewPassword",
        };

        // Act
        await _handler.Handle(request, CancellationToken.None);

        // Assert
        await using var readContext = VendingMachineDbContextFactory.Create();
        var storedUser = await readContext.Users.SingleOrDefaultAsync();
        storedUser.Should().NotBeNull();
        storedUser?.Id.Should().Be(user.Id);
        storedUser?.Username.Should().Be(user.Username);
        storedUser?.Password.Should().Be(new Password(request.Password));
        storedUser?.Role.Should().Be(user.Role);
    }
}