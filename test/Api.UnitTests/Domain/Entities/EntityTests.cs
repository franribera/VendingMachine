using Api.Domain.Entities;
using Api.Domain.Enumerations;
using Api.Domain.ValueObjects;
using FluentAssertions;
using Xunit;

namespace Api.UnitTests.Domain.Entities;

public class EntityTests
{
    private class TestEntity : Entity
    {
        public string Name { get; }
        public int Amount { get; }

        public TestEntity(string name, int amount)
        {
            Name = name;
            Amount = amount;
        }

        public void SetId(long value)
        {
            Id = value;
        }
    }

    [Fact]
    public void Not_equals_if_other_is_not_an_entity()
    {
        // Arrange
        var username = "RandomName";
        var entity = new TestEntity("Name", 123);

        // Act
        var result = entity.Equals(username);

        // Assert
        result.Should().BeFalse();
    }
    
    [Fact]
    public void Not_equals_to_other_entity_types()
    {
        // Arrange
        var user = new User("Name", "Password", Role.Buyer.Name);
        var entity = new TestEntity("Name", 123);

        // Act
        var result = entity.Equals(user);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Not_equals_if_entity_is_transient()
    {
        // Arrange
        var entity1 = new TestEntity("Name", 123);
        var entity2 = new TestEntity("Name", 123);
        entity2.SetId(11);

        // Act
        var result = entity1.Equals(entity2);
        var operatorResult = entity1 == entity2;

        // Assert
        result.Should().BeFalse();
        operatorResult.Should().BeFalse();
    }

    [Fact]
    public void Not_equals_if_other_entity_is_transient()
    {
        // Arrange
        var entity1 = new TestEntity("Name", 123);
        entity1.SetId(11);
        var entity2 = new TestEntity("Name", 123);

        // Act
        var result = entity1.Equals(entity2);
        var operatorResult = entity1 == entity2;

        // Assert
        result.Should().BeFalse();
        operatorResult.Should().BeFalse();
    }

    [Fact]
    public void Not_equals_if_other_entity_has_different_id()
    {
        // Arrange
        var entity1 = new TestEntity("Name", 123);
        entity1.SetId(11);
        var entity2 = new TestEntity("Name", 123);
        entity2.SetId(22);

        // Act
        var result = entity1.Equals(entity2);
        var operatorResult = entity1 == entity2;

        // Assert
        result.Should().BeFalse();
        operatorResult.Should().BeFalse();
    }

    [Fact]
    public void Equals_to_other_references_of_same_entity()
    {
        // Arrange
        var entity1 = new TestEntity("Name", 123);
        entity1.SetId(11);
        var entity2 = entity1;

        // Act
        var result = entity1.Equals(entity2);
        var operatorResult = entity1 == entity2;

        // Assert
        result.Should().BeTrue();
        operatorResult.Should().BeTrue();
    }

    [Fact]
    public void Equals_to_other_references_of_same_entity_with_same_id()
    {
        // Arrange
        var entity1 = new TestEntity("Name", 123);
        entity1.SetId(11);
        var entity2 = new TestEntity("Name", 123);
        entity2.SetId(11);

        // Act
        var result = entity1.Equals(entity2);
        var operatorResult = entity1 == entity2;

        // Assert
        result.Should().BeTrue();
        operatorResult.Should().BeTrue();
    }
}