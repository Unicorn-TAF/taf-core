using Unicorn.Taf.Core.Testing.Attributes;
using Unicorn.Taf.Core.Verification.Matchers;
using Unicorn.UnitTests.BO;

namespace Unicorn.UnitTests.Suites
{
    [Disabled("By design", nameof(IsSuiteDisabled))]
    [Suite("Suite disabled by some condition")]
    [Tag(Tag.DisabledByCondition)]
    public class USuiteDisabledWithCondition : UBaseTestSuite
    {
        public static bool IsTest1Disabled { get; set; }
        public static bool IsTest2Disabled { get; set; }
        public static bool IsSuiteDisabled { get; set; }

        public static string Output { get; set; }

        [Test]
        [Disabled("By design", nameof(IsTest2Disabled))]
        public void Test2()
        {
            Output += "Test2";
            Do.Assertion.StartAssertionsChain(null);
        }

        [Test]
        [Disabled("By design", nameof(IsTest1Disabled))]
        public void Test1()
        {
            Output += "Test1";
            Do.Assertion.AssertThat(new SampleObject(), Is.Null());
        }
    }
}
