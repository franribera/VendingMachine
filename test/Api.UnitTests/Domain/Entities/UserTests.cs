using System;
using System.Linq;
using Api.Domain.Entities;
using Api.Domain.Enumerations;
using Api.Domain.ValueObjects;
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
        user.DepositMoney(Coin.HundredCent);
        user.DepositMoney(Coin.HundredCent);
        user.DepositMoney(Coin.HundredCent);

        // Assert
        user.Deposit.Amount.Should().Be(300);
    }

    [Fact]
    public void Empties_deposit()
    {
        // Arrange
        var user = new User("Username", "Password", Role.Buyer.Name);
        user.DepositMoney(Coin.FiftyCent);
        user.DepositMoney(Coin.FiftyCent);
        user.DepositMoney(Coin.FiftyCent);

        // Act
        var coins = user.ResetDeposit();

        // Assert
        coins.Sum(c => c.Value).Should().Be(150);
        user.Deposit.Amount.Should().Be(0);
    }

    [Fact]
    public void Throws_if_there_is_not_enough_money_to_withdraw()
    {
        // Arrange
        var user = new User("Username", "Password", Role.Buyer.Name);
        user.DepositMoney(Coin.FiftyCent);

        var withdraw = () => user.Withdraw(new Money(Coin.HundredCent.Value));

        // Act - Assert
        withdraw.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Withdraws_if_there_is_enough_money()
    {
        // Arrange
        var user = new User("Username", "Password", Role.Buyer.Name);
        user.DepositMoney(Coin.FiftyCent);

        // Act
        user.Withdraw(new Money(Coin.TwentyCent.Value));

        // Assert
        user.Deposit.Amount.Should().Be(30);
    }
}