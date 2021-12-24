using System.Threading;
using System.Threading.Tasks;
using Api.Domain.Entities;
using Api.Domain.Enumerations;
using Api.Features.Users.Read;
using Api.UnitTests.Fixtures;
using FluentAssertions;
using Xunit;

namespace Api.UnitTests.Features.Users;

[Collection(nameof(TestFixture)), ResetDatabase]
public class ReadUserRequestTests
{
    private readonly ReadUserRequestHandler _handler;

    public ReadUserRequestTests(TestFixture testFixture)
    {
        _handler = new ReadUserRequestHandler(testFixture.DbContext);
    }

    [Fact]
    public async Task Returns_the_user()
    {
        // Arrange
        var user = new User("Username", "Password", Role.Seller.Name);

        await using (var writeContext = VendingMachineDbContextFactory.Create())
        {
            await writeContext.Users.AddAsync(user);
            await writeContext.SaveChangesAsync();
        }

        var request = new ReadUserRequest { UserId = user.Id };

        // Act
        var response = await _handler.Handle(request, CancellationToken.None);

        // Assert
        response.Should().NotBeNull();
        response.Id.Should().Be(user.Id);
        response.Username.Should().Be(user.Username);
        response.Role.Should().Be(user.Role.Name);
    }
}