using System;
using Api.Domain.ValueObjects;
using FluentAssertions;
using Xunit;

namespace Api.UnitTests.Domain.ValueObjects;

public class PasswordTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Cannot_be_empty(string value)
    {
        // Arrange
        Action constructor = () => new Password(value);

        // Act - Assert
        constructor.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Cannot_have_less_than_6_characters()
    {
        // Arrange
        Action constructor = () => new Password("12345");

        // Act - Assert
        constructor.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Two_passwords_are_equal_if_have_same_value()
    {
        // Arrange
        var password1 = new Password("123456");
        var password2 = new Password("123456");

        // Assert
        password1.Should().Be(password2);
    }

    [Fact]
    public void Two_passwords_are_not_equal_if_have_different_value()
    {
        // Arrange
        var password1 = new Password("123456");
        var password2 = new Password("1234567");

        // Assert
        password1.Should().NotBe(password2);
    }
}