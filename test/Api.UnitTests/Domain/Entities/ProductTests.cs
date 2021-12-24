using System;
using Api.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Api.UnitTests.Domain.Entities;

public class ProductTests
{
    [Fact]
    public void Reprices_as_expected()
    {
        // Arrange
        var product = new Product("Whatever", 123, 1, 100);

        // Act
        product.Reprice(85);

        // Assert
        product.Price.Amount.Should().Be(85);
    }

    [Fact]
    public void Replenishes_as_expected()
    {
        // Arrange
        var product = new Product("Whatever", 123, 1, 100);

        // Act
        product.Replenish(25);

        // Assert
        product.Quantity.Should().Be(26);
    }

    [Fact]
    public void Throws_if_is_understocked()
    {
        // Arrange
        var product = new Product("Whatever", 123, 20, 100);

        var take = () => product.Take(25);

        // Act - Assert
        take.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Takes_and_returns_the_cost()
    {
        // Arrange
        var product = new Product("Whatever", 123, 20, 100);

        // Act
        var cost = product.Take(5);

        // Assert
        cost.Amount.Should().Be(500);
        product.Quantity.Should().Be(15);
    }
}