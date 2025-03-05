using NUnit.Framework;
using System.Reflection;
using Unicorn.Taf.Core;
using Unicorn.Taf.Core.Engine;
using Unicorn.Taf.Core.Steps;
using Unicorn.Taf.Core.Testing;
using Unicorn.UnitTests.Util;

namespace Unicorn.UnitTests.Tests.Core.Testing
{
    [TestFixture]
    public class StepsFeature : NUnitTestRunner
    {
        private static string Output = string.Empty;

        [OneTimeSetUp]
        public static void SetConfig()
        {
            Config.TestsExecutionOrder = TestsOrder.Declaration;
            Config.SetSuiteTags(Tag.Steps);
            TafEvents.OnStepStart += ReportInfo;
        }

        [TearDown]
        public void CleanOutput() =>
            Output = string.Empty;

        [OneTimeTearDown]
        public static void ResetConfig()
        {
            Config.Reset();
            TafEvents.OnStepStart -= ReportInfo;
            Output = null;
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check step events")]
        public void TestStepEvents()
        {
            Config.SetTestCategories("1");

            new TestsRunner(Assembly.GetExecutingAssembly(), false).RunTests();
            Assert.That(
                Output, 
                Is.EqualTo("Test1AfterTestAssert that Test2 Is equal to Test2AfterTestAfterSuite"));
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check step events")]
        public void TestStepWithFormat()
        {
            Config.SetTestCategories("2");

            new TestsRunner(Assembly.GetExecutingAssembly(), false).RunTests();
            Assert.That(
                Output,
                Is.EqualTo("NumFormat 2.1AfterTestAfterSuite"));
        }

        private static void ReportInfo(MethodBase method, object[] arguments) =>
            Output += StepsUtilities.GetStepInfo(method, arguments);
    }
}
