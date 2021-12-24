using System.Linq;
using Api.Domain.Entities;
using Api.Domain.Enumerations;
using FluentAssertions;
using Xunit;

namespace Api.UnitTests.Domain.Entities;

public class UserTests
{
    [Fact]
    public void Deposits_money()
    {
        // Arrange
        var user = new User("Username", "Password", Role.Buyer.Name);

        // Act
        user.DepositMoney(100);
        user.DepositMoney(100);
        user.DepositMoney(100);

        // Assert
        user.Deposit.Amount.Should().Be(300);
    }

    [Fact]
    public void Empties_deposit()
    {
        // Arrange
        var user = new User("Username", "Password", Role.Buyer.Name);
        user.DepositMoney(50);
        user.DepositMoney(50);
        user.DepositMoney(50);

        // Act
        var coins = user.ResetDeposit();

        // Assert
        coins.Sum(c => c.Value).Should().Be(150);
        user.Deposit.Amount.Should().Be(0);
    }
}