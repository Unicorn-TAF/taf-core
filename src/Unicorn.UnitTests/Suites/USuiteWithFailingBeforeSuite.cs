using Unicorn.Taf.Core.Testing.Attributes;
using Unicorn.Taf.Core.Verification.Matchers;
using Unicorn.UnitTests.BO;
using System;

namespace Unicorn.UnitTests.Suites
{
    [Suite]
    [Tag(Tag.FailBeforeSuite)]
    public class USuiteWithFailingBeforeSuite : UBaseTestSuite
    {
        [BeforeSuite]
        public void BeforeSuite() =>
            throw new ArgumentException("This is intended.");

        [Test]
        public void Test1() =>
            Do.Assertion.StartAssertionsChain("");
    }
}
