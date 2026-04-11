using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Unicorn.Taf.Core;
using Unicorn.Taf.Core.Engine;
using Unicorn.Taf.Core.Testing;
using Unicorn.UnitTests.Util;

namespace Unicorn.UnitTests.Tests.Core.Engine
{
    [TestFixture]
    public class OverrideConfigFromEnvVars : NUnitTestRunner
    {
        private const string ConfigContent = @"{""parallel"": ""none"",""threads"": 3,""testTimeout"": " +
            @"25,""suiteTimeout"": 55,""testsDependency"":""skip"", ""testsOrder"": ""Declaration""," +
            @"""tags"": [ ""feature1"", ""feature1"" ],""categories"": [ ""category"" ],""tests"": [ ]," +
            @"""userDefined"": {""customSetting1"": ""customValue1"", ""customSetting3"": ""customValue3""} }";

        private static TestsRunner runner;

        [OneTimeSetUp]
        public static void Setup()
        {
            Environment.SetEnvironmentVariable("UNICORN_TEST_TIMEOUT", "0.5");
            Environment.SetEnvironmentVariable("UNICORN_SUITE_TIMEOUT", "5");
            Environment.SetEnvironmentVariable("UNICORN_PARALLEL_BY", "Suite");
            Environment.SetEnvironmentVariable("UNICORN_THREADS", "5");
            Environment.SetEnvironmentVariable("UNICORN_TESTS_DEPENDENCY", "DoNotRun");
            File.WriteAllText(ConfigName, ConfigContent);
            runner = new TestsRunner(Assembly.GetExecutingAssembly(), ConfigName);
            runner.RunTests();
        }

        [OneTimeTearDown]
        public static void Cleanup()
        {
            Environment.SetEnvironmentVariable("UNICORN_TEST_TIMEOUT", null);
            Environment.SetEnvironmentVariable("UNICORN_SUITE_TIMEOUT", null);
            Environment.SetEnvironmentVariable("UNICORN_PARALLEL_BY", null);
            Environment.SetEnvironmentVariable("UNICORN_THREADS", null);
            Environment.SetEnvironmentVariable("UNICORN_TESTS_DEPENDENCY", null);
            Config.Reset();
            runner = null;
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Test config test timeout override")]
        public void TestConfigTestTimeoutOverride() =>
            Assert.That(Config.TestTimeout, Is.EqualTo(TimeSpan.FromMinutes(0.5)));

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Test config suite timeout override")]
        public void TestConfigSuiteTimeoutOverride() =>
            Assert.That(Config.SuiteTimeout, Is.EqualTo(TimeSpan.FromMinutes(5)));

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Test config parallel override")]
        public void TestConfigParallelOverride() =>
            Assert.That(Config.ParallelBy, Is.EqualTo(Parallelization.Suite));

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Test config threads override")]
        public void TestConfigThreadsOverride() =>
            Assert.That(Config.Threads, Is.EqualTo(5));

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Test config dependent tests override")]
        public void TestConfigDependentTestsOverride() =>
            Assert.That(Config.DependentTests, Is.EqualTo(TestsDependency.DoNotRun));
    }
}
