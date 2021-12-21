using Xunit;

namespace Api.UnitTests.Fixtures;

[CollectionDefinition(nameof(TestFixture))]
public class TestCollection : ICollectionFixture<TestFixture>
{
    
}