using System;
using Api.Domain.ValueObjects;
using FluentAssertions;
using Xunit;

namespace Api.UnitTests.Domain.ValueObjects;

public class PasswordTests
{
    [Fact]
    public void Performs_hash()
    {
        // Act
        var password = new Password("123456");

        // Assert
        password.Value.Should().Be("jZae727K08KaOmKSgOaGzww/XVqGr/PKEgIMkjrcbJI=");
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