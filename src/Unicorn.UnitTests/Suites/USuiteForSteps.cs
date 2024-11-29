using Unicorn.Taf.Core.Testing;
using Unicorn.Taf.Core.Testing.Attributes;
using Unicorn.Taf.Core.Verification.Matchers;

namespace Unicorn.UnitTests.Suites
{
    [Suite("Tests for reporting")]
    [Tag(Tag.Steps)]
    public class USuiteForSteps : UBaseTestSuite
    {
        [Category("1")]
        [Test]
        public void Test1() =>
            Do.Assertion.StartAssertionsChain("Test1");

        [Category("1")]
        [Test]
        public void Test2() =>
            Do.Assertion.AssertThat("Test2", Is.EqualTo("Test2"));

        [Category("1")]
        [Test]
        [Disabled("")]
        public void TestToSkip() =>
            Do.Assertion.StartAssertionsChain("TestToSkip");

        [Category("2")]
        [Test]
        public void TestWithFormattingStep() =>
            Do.StepWithFormatting(2.1234567d);

        [AfterTest]
        public void AfterTest() =>
            Do.Assertion.StartAssertionsChain("AfterTest");

        [AfterSuite]
        public void AfterSuite() =>
            Do.Assertion.StartAssertionsChain("AfterSuite");
    }
}
