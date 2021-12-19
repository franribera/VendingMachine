using Api.Domain.ValueObjects;
using FluentAssertions;
using Xunit;

namespace Api.UnitTests.Domain.ValueObjects;

public class CoinTests
{
    [Fact]
    public void Two_coins_are_equal_if_have_same_value()
    {
        // Arrange
        var coin1 = Coin.TwentyCent;
        var coin2 = Coin.TwentyCent;

        // Act - Assert
        coin1.Should().Be(coin2);
    }

    [Fact]
    public void Two_coins_are_not_equal_if_have_different_value()
    {
        // Arrange
        var coin1 = Coin.TwentyCent;
        var coin2 = Coin.HundredCent;

        // Act - Assert
        coin1.Should().NotBe(coin2);
    }

    [Theory]
    [InlineData(0, true)]
    [InlineData(1, false)]
    [InlineData(5, true)]
    [InlineData(10, true)]
    [InlineData(15, false)]
    [InlineData(20, true)]
    [InlineData(50, true)]
    [InlineData(100, true)]
    public void Checks_if_coin_value_is_allowed(int value, bool allowed)
    {
        // Act
        var result = Coin.IsAllowed(value);

        // Assert
        result.Should().Be(allowed);
    }
}
