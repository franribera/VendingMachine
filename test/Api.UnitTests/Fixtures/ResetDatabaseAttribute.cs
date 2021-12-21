using System.Reflection;
using Xunit.Sdk;

namespace Api.UnitTests.Fixtures;

public class ResetDatabaseAttribute : BeforeAfterTestAttribute
{
    public override void Before(MethodInfo methodUnderTest)
    {
        TestFixture.ResetDatabase();
    }
}