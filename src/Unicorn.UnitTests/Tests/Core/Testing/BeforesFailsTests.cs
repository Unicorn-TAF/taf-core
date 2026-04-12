using NUnit.Framework;
using System;
using Unicorn.Taf.Core.Utility;
using Unicorn.UnitTests.Util;
using Unicorn.Taf.Core.Logging;
using Unicorn.Taf.Core;
using Unicorn.Taf.Core.Engine;
using System.Reflection;

namespace Unicorn.UnitTests.Tests.Core.Utility
{
    [TestFixture]
    public class BeforesFailsTests : NUnitTestRunner
    {
        [OneTimeTearDown]
        public static void ResetConfig() =>
            Config.Reset();

        [Test(Description = "Check tests skip reason when BeforeSuite failed")]
        public void CheckSkipReasonWhenBeforeSuiteFailed()
        {
            Config.SetSuiteTags(Tag.FailBeforeSuite);
            var runner = new TestsRunner(Assembly.GetExecutingAssembly(), false);
            runner.RunTests();
            Assert.That(runner.Outcome.SuitesOutcomes[0].TestsOutcomes[0].FailMessage, Is.EqualTo("BeforeSuite was failed. Error: This is intended."));
        }

        [Test(Description = "Check tests skip reason when BeforeTest failed")]
        public void CheckSkipReasonWhenBeforeTestFailed()
        {
            Config.SetSuiteTags(Tag.FailBeforeTest);
            var runner = new TestsRunner(Assembly.GetExecutingAssembly(), false);
            var outcome = runner.RunTests();
            Assert.That(runner.Outcome.SuitesOutcomes[0].TestsOutcomes[0].FailMessage, Is.EqualTo("BeforeTest was failed. Error: This is intended."));
        }
    }
}
