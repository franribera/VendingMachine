using System.Collections.Generic;
using Api.Domain.ValueObjects;
using FluentAssertions;
using Xunit;

namespace Api.UnitTests.Domain.ValueObjects;

public class ValueObjectTests
{
    private class TestValueObject : ValueObject
    {
        public string Name { get; }
        public int Amount { get; }

        public TestValueObject(string name, int amount)
        {
            Name = name;
            Amount = amount;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Name;
            yield return Amount;
        }
    }

    [Fact]
    public void Not_equals_to_null()
    {
        // Arrange
        var testObject = new TestValueObject("aaa", 123);

        // Act
        var result = testObject.Equals(null);
        var operatorResult = testObject == null;

        // Assert
        result.Should().BeFalse();
        operatorResult.Should().BeFalse();
    }

    [Fact]
    public void Equals_if_different_references_have_same_values()
    {
        // Arrange
        var testObject1 = new TestValueObject("aaa", 123);
        var testObject2 = new TestValueObject("aaa", 123);

        // Act
        var result = testObject1.Equals(testObject2);
        var operatorResult = testObject1 == testObject2;

        // Assert
        operatorResult.Should().BeTrue();
    }

    [Fact]
    public void Not_equals_if_different_references_have_different_values()
    {
        // Arrange
        var testObject1 = new TestValueObject("aaa", 123);
        var testObject2 = new TestValueObject("bbb", 123);

        // Act
        var result = testObject1.Equals(testObject2);
        var operatorResult = testObject1 == testObject2;

        // Assert
        operatorResult.Should().BeFalse();
    }
}