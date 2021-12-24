using System.Threading;
using System.Threading.Tasks;
using Api.Domain.Entities;
using Api.Domain.Enumerations;
using Api.Domain.ValueObjects;
using Api.Features.Users.Deposit;
using Api.UnitTests.Fixtures;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Api.UnitTests.Features.Users;

[Collection(nameof(TestFixture)), ResetDatabase]
public class DepositRequestTests
{
    private readonly DepositRequestHandler _handler;

    public DepositRequestTests(TestFixture testFixture)
    {
        _handler = new DepositRequestHandler(testFixture.DbContext);
    }

    [Fact]
    public async Task Adds_coin_to_the_user_deposit()
    {
        // Arrange
        var user = new User("Username", "Password", Role.Buyer.Name);
        user.DepositMoney(Coin.TenCent);

        await using (var writeContext = VendingMachineDbContextFactory.Create())
        {
            await writeContext.Users.AddAsync(user);
            await writeContext.SaveChangesAsync();
        }

        var request = new DepositRequest()
        {
            UserId = user.Id,
            Coin = 20
        };

        // Act
        await _handler.Handle(request, CancellationToken.None);

        // Assert
        await using var readContext = VendingMachineDbContextFactory.Create();
        var storedUser = await readContext.Users.SingleOrDefaultAsync();
        storedUser.Should().NotBeNull();
        storedUser.Deposit.Should().Be(new Money(30));
    }
}