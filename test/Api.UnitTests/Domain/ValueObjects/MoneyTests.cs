using System.Linq;
using Api.Domain.ValueObjects;
using FluentAssertions;
using Xunit;

namespace Api.UnitTests.Domain.ValueObjects;

public class MoneyTests
{
    [Fact]
    public void Two_money_instances_equals_if_have_the_same_amount()
    {
        // Arrange
        var money1 = new Money(123);
        var money2 = new Money(123);

        // Act
        var result = money1.Equals(money2);
        var operatorResult = money1 == money2;

        // Assert
        result.Should().BeTrue();
        operatorResult.Should().BeTrue();
    }

    [Fact]
    public void Sums_money_as_expected()
    {
        // Arrange
        var money1 = new Money(111);
        var money2 = new Money(222);

        // Act
        var result = money1 + money2;

        // Assert
        result.Amount.Should().Be(333);
    }

    [Fact]
    public void Sums_money_and_coin_as_expected()
    {
        // Arrange
        var money = new Money(100);
        
        // Act
        var result = money + Coin.FiftyCent;

        // Assert
        result.Amount.Should().Be(150);
    }

    [Fact]
    public void Subtract_as_expected()
    {
        // Arrange
        var money1 = new Money(333);
        var money2 = new Money(222);

        // Act
        var result = money1 - money2;

        // Assert
        result.Amount.Should().Be(111);
    }

    [Theory]
    [InlineData(15, 0, 0, 0, 1, 1)]
    [InlineData(25, 0, 0, 1, 0, 1)]
    [InlineData(95, 0, 1, 2, 0, 1)]
    [InlineData(285, 2, 1, 1, 1, 1)]
    [InlineData(505, 5, 0, 0, 0, 1)]
    public void Allocates_with_higher_coins_priority(int amount, int hundredCentCount, int fiftyCentCount, int twentyCentCount, int tenCentCount, int fiveCentCount)
    {
        // Arrange
        var money = new Money(amount);

        // Act
        var coins = money.Allocate().ToList();

        // Assert
        coins.Count(c => c == Coin.HundredCent).Should().Be(hundredCentCount);
        coins.Count(c => c == Coin.FiftyCent).Should().Be(fiftyCentCount);
        coins.Count(c => c == Coin.TwentyCent).Should().Be(twentyCentCount);
        coins.Count(c => c == Coin.TenCent).Should().Be(tenCentCount);
        coins.Count(c => c == Coin.FiveCent).Should().Be(fiveCentCount);
    }
}