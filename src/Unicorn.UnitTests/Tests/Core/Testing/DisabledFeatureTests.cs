using NUnit.Framework;
using System.Reflection;
using Unicorn.Taf.Core;
using Unicorn.Taf.Core.Engine;
using Unicorn.Taf.Core.Testing;
using Unicorn.UnitTests.Suites;
using Unicorn.UnitTests.Util;

namespace Unicorn.UnitTests.Tests.Core.Testing
{
    [TestFixture]
    public class DisabledFeatureTests : NUnitTestRunner
    {
        [OneTimeSetUp]
        public static void SetUp()
        {
            Config.SetTestCategories();
            Config.TestsExecutionOrder = TestsOrder.Declaration;
        }

        [OneTimeTearDown]
        public static void ResetConfig()
        {
            Config.Reset();
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Test For Suite Disabling")]
        public void TestSuitesDisabledSuite()
        {
            USuiteDisabled.Output = string.Empty;
            Config.SetSuiteTags(Tag.Disabled);
            TestsRunner runner = new TestsRunner(Assembly.GetExecutingAssembly(), false);
            runner.RunTests();
            Assert.That(USuiteDisabled.Output, Is.EqualTo(string.Empty));
        }

        [Author("Vitaliy Dobriyan")]
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void TestSuitesDisabledSuiteByCondition(bool disabled)
        {
            USuiteDisabledWithCondition.IsSuiteDisabled = disabled;
            USuiteDisabledWithCondition.IsTest1Disabled = false;
            USuiteDisabledWithCondition.IsTest2Disabled = false;
            USuiteDisabledWithCondition.Output = "";
            Config.SetSuiteTags(Tag.DisabledByCondition);
            TestsRunner runner = new TestsRunner(Assembly.GetExecutingAssembly(), false);
            runner.RunTests();

            string expected = disabled ? string.Empty : "Test2Test1";
            Assert.That(USuiteDisabledWithCondition.Output, Is.EqualTo(expected));
        }

        [Author("Vitaliy Dobriyan")]
        [Test]
        [TestCase(true, false)]
        [TestCase(false, true)]
        public void TestTestsDisabledSuiteByCondition(bool disableTest1, bool disableTest2)
        {
            USuiteDisabledWithCondition.IsSuiteDisabled = false;
            USuiteDisabledWithCondition.IsTest1Disabled = disableTest1;
            USuiteDisabledWithCondition.IsTest2Disabled = disableTest2;
            USuiteDisabledWithCondition.Output = "";
            Config.SetSuiteTags(Tag.DisabledByCondition);
            TestsRunner runner = new TestsRunner(Assembly.GetExecutingAssembly(), false);
            runner.RunTests();

            string expected = $"{(disableTest1 ? "" : "Test1")}{(disableTest2 ? "" : "Test2")}";
            Assert.That(USuiteDisabledWithCondition.Output, Is.EqualTo(expected));
        }
    }
}
