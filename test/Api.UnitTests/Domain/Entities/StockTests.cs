using System;
using Api.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Api.UnitTests.Domain.Entities;

public class StockTests
{
    [Fact]
    public void Reprices_as_expected()
    {
        // Arrange
        var stock = new Stock(1, 1, 100);

        // Act
        stock.Reprice(85);

        // Assert
        stock.Price.Amount.Should().Be(85);
    }

    [Fact]
    public void Replenishes_as_expected()
    {
        // Arrange
        var stock = new Stock(1, 1, 100);

        // Act
        stock.Replenish(25);

        // Assert
        stock.Quantity.Should().Be(26);
    }

    [Fact]
    public void Throws_if_is_understocked()
    {
        // Arrange
        var stock = new Stock(1, 20, 100);

        var take = () => stock.Take(25);

        // Act - Assert
        take.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Takes_and_returns_the_cost()
    {
        // Arrange
        var stock = new Stock(1, 20, 100);

        // Act
        var cost = stock.Take(5);

        // Assert
        cost.Amount.Should().Be(500);
        stock.Quantity.Should().Be(15);
    }
}