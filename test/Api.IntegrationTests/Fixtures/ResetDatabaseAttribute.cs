using System.Reflection;
using Xunit.Sdk;

namespace Api.IntegrationTests.Fixtures;

public class ResetDatabaseAttribute : BeforeAfterTestAttribute
{
    public override void Before(MethodInfo methodUnderTest)
    {
        TestFixture.ResetDatabase();
    }

    public override void After(MethodInfo methodUnderTest)
    {
        TestFixture.ResetDatabase();
    }
}