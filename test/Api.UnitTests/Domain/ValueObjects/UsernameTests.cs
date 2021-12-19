using System;
using Api.Domain.ValueObjects;
using FluentAssertions;
using Xunit;

namespace Api.UnitTests.Domain.ValueObjects;

public class UsernameTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Cannot_be_empty(string value)
    {
        // Arrange
        Action constructor = () => new Username(value);

        // Act - Assert
        constructor.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Two_usernames_are_equal_if_have_same_value()
    {
        // Arrange
        var username1 = new Username("aaa");
        var username2 = new Username("aaa");

        // Assert
        username1.Should().Be(username2);
    }

    [Fact]
    public void Two_usernames_are_not_equal_if_have_different_value()
    {
        // Arrange
        var username1 = new Username("aaa");
        var username2 = new Username("bbb");

        // Assert
        username1.Should().NotBe(username2);
    }
}