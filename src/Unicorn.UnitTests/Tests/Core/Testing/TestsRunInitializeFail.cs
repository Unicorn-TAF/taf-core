﻿using NUnit.Framework;
using System.Collections.Generic;
using System.Reflection;
using Unicorn.Taf.Core.Engine;
using Unicorn.Taf.Core.Testing;
using Unicorn.UnitTests.Suites;
using Unicorn.UnitTests.Util;

namespace Unicorn.UnitTests.Tests.Core.Testing
{
    [TestFixture]
    public class TestsRunInitializeFail : NUnitTestRunner
    {
        [SetUp]
        public void Setup() =>
            UTestsAssembly.FailRunInit = true;

        [TearDown]
        public void Cleanup() =>
            UTestsAssembly.FailRunInit = false;

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check ordered targeted runner behavior on suite init fail")]
        public void TestOrderedTargetedRunnerBehaviorOnSuiteInitFail()
        {
            var filters = new Dictionary<string, string>
            {
                { "Ordered suite 2", "category2" },
            };

            var runner = new PlaylistRunner(Assembly.GetExecutingAssembly(), filters);
            runner.RunTests();

            Assert.IsFalse(runner.Outcome.RunInitialized);
            Assert.That(runner.Outcome.RunStatus, Is.EqualTo(Status.Failed));
            Assert.That(runner.Outcome.InitFailMessage, Is.EqualTo("Run init failed"));
            Assert.That(runner.Outcome.SuitesOutcomes.Count, Is.EqualTo(0));
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check runner behavior on suite init fail")]
        public void TestRunnerBehaviorOnSuiteInitFail()
        {
            var runner = new TestsRunner(Assembly.GetExecutingAssembly(), false);
            runner.RunTests();

            Assert.IsFalse(runner.Outcome.RunInitialized);
            Assert.That(runner.Outcome.RunStatus, Is.EqualTo(Status.Failed));
            Assert.That(runner.Outcome.InitFailMessage, Is.EqualTo("Run init failed"));
            Assert.That(runner.Outcome.SuitesOutcomes.Count, Is.EqualTo(0));
        }
    }
}
