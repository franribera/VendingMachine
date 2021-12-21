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

namespace Api.UnitTests.Features.Users.Delete;

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
        var user = new User("Username", "Password", Role.Seller.Name);

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
}