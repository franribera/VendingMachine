using Xunit;

namespace Api.IntegrationTests.Fixtures;

[CollectionDefinition(nameof(TestFixture))]
public class TestCollection : ICollectionFixture<TestFixture>
{
    
}