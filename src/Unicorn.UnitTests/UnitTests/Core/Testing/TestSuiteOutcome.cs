using NUnit.Framework;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Unicorn.Taf.Core;
using Unicorn.Taf.Core.Engine;
using Unicorn.Taf.Core.Testing;
using Unicorn.UnitTests.Util;

namespace Unicorn.UnitTests.Core.Testing
{
    [TestFixture]
    public class TestSuiteOutcome : NUnitTestRunner
    {
        private static TestsRunner runner;
        private static TimeSpan runTime;

        private static SuiteOutcome Outcome => runner.Outcome.SuitesOutcomes[0];

        [OneTimeSetUp]
        public static void SetUp()
        {
            Config.SetSuiteTags(Tag.Sample);
            runner = new TestsRunner(Assembly.GetExecutingAssembly(), false);

            var timer = Stopwatch.StartNew();
            runner.RunTests();

            runTime = timer.Elapsed;
        }

        [OneTimeTearDown]
        public static void TearDown()
        {
            Config.Reset();
            runner = null;
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check Author attribute")]
        public void TestAuthorAttribute()
        {
            Assert.That(Outcome.TestsOutcomes.First(o => o.Title.Equals("Test1")).Author, Is.EqualTo("No author"));
            Assert.That(Outcome.TestsOutcomes.First(o => o.Title.Equals("Test2")).Author, Is.EqualTo("Author2"));
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check TestCaseId attribute")]
        public void TestCaseIdAttribute()
        {
            Assert.That(Outcome.TestsOutcomes.First(o => o.Title.Equals("Test1")).TestCaseId, Is.EqualTo("TC1"));
            Assert.That(Outcome.TestsOutcomes.First(o => o.Title.Equals("Test2")).TestCaseId, Is.Null);
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check suite outcome TotalTests counter")]
        public void TestSuiteOutcomeTotalTestsCounter() =>
            Assert.That(Outcome.TotalTests, Is.EqualTo(3));

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check suite outcome ExecutionTime")]
        public void TestSuiteOutcomeExecutionTime()
        {
            Assert.That(Outcome.ExecutionTime, Is.GreaterThan(TimeSpan.Zero));
            Assert.That(Outcome.ExecutionTime, Is.Not.GreaterThan(runTime));
            Assert.That(Outcome.TestsOutcomes[0].ExecutionTime, Is.GreaterThan(TimeSpan.Zero));
            Assert.That(Outcome.TestsOutcomes[0].ExecutionTime, Is.Not.GreaterThan(runTime));
            Assert.That(Outcome.TestsOutcomes[1].ExecutionTime, Is.GreaterThan(TimeSpan.Zero));
            Assert.That(Outcome.TestsOutcomes[1].ExecutionTime, Is.Not.GreaterThan(runTime));
        }
    }
}
