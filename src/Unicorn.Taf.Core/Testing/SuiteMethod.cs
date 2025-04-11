﻿using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using Unicorn.Taf.Core.Logging;
using Unicorn.Taf.Core.Testing.Attributes;

namespace Unicorn.Taf.Core.Testing
{
    /// <summary>
    /// Represents suite method type
    /// </summary>
    public enum SuiteMethodType
    {
        /// <summary>
        /// Method executed before all suite tests.
        /// </summary>
        BeforeSuite,

        /// <summary>
        /// Method executed before each suite test.
        /// </summary>
        BeforeTest,

        /// <summary>
        /// Method executed after each suite test.
        /// </summary>
        AfterTest,

        /// <summary>
        /// Method executed after all suite tests.
        /// </summary>
        AfterSuite,

        /// <summary>
        /// Test itself.
        /// </summary>
        Test
    }

    /// <summary>
    /// Represents the suite method itself (which is also base for <see cref="Test"/>).
    /// Contains list of events related to different suite method states (started, finished, skipped, passed, failed)<para/>
    /// Contains methods to execute the suite method and base methods for all types of suite methods<para/>
    /// </summary>
    public class SuiteMethod
    {
        private const string NoAuthor = "No author";

        /// <summary>
        /// Initializes a new instance of the <see cref="SuiteMethod"/> class, which is part of some TestSuite.
        /// Contains list of events related to different Test states (started, finished, skipped, passed, failed)
        /// Contains methods to execute the test and check if test should be skipped
        /// </summary>
        /// <param name="testMethod">MethodInfo instance which represents test method</param>
        public SuiteMethod(MethodInfo testMethod)
        {
            TestMethod = testMethod;
            Outcome = new TestOutcome
            {
                FullMethodName = AdapterUtilities.GetFullTestMethodName(testMethod),
            };

            var testAttribute = TestMethod.GetCustomAttribute<TestAttribute>(true);
            var authorAttribute = TestMethod.GetCustomAttribute<AuthorAttribute>(true);
            Outcome.Author = authorAttribute == null ? NoAuthor : authorAttribute.Author;

            Outcome.Title = string.IsNullOrEmpty(testAttribute?.Title) ?
                TestMethod.Name :
                testAttribute.Title;
        }

        /// <summary>
        /// Gets or sets current test outcome, contains base information about execution results
        /// </summary>
        public TestOutcome Outcome { get; set; }

        /// <summary>
        /// Gets or sets current suite method type
        /// </summary>
        public SuiteMethodType MethodType { get; set; }

        /// <summary>
        /// Gets or sets <see cref="MethodInfo"/> which represents test
        /// </summary>
        public MethodInfo TestMethod { get; set; }

        /// <summary>
        /// Gets or sets test method execution timer
        /// </summary>
        protected Stopwatch TestTimer { get; set; }

        /// <summary>
        /// Execute current test and fill <see cref="TestOutcome"/><para/>
        /// Before the test List of BeforeTests is executed<para/>
        /// After the test List of AfterTests is executed
        /// </summary>
        /// <param name="suiteInstance">test suite instance to run in</param>
        public virtual void Execute(TestSuite suiteInstance)
        {
            ULog.Info("---- {0} '{1}'", MethodType, Outcome.Title);

            TafEvents.CallOnSuiteMethodStart(this);

            Outcome.StartTime = DateTime.Now;
            TestTimer = Stopwatch.StartNew();

            RunSuiteMethod(suiteInstance);

            TestTimer.Stop();
            Outcome.ExecutionTime = TestTimer.Elapsed;

            LogStatus();
            TafEvents.CallOnSuiteMethodFinish(this);
        }

        /// <summary>
        /// Fail test, fill <see cref="TestOutcome"/> exception and bugs and invoke onFail event.<para/>
        /// If test failed not by existing bug it is marked as 'To investigate'
        /// </summary>
        /// <param name="ex">Exception caught on test execution</param>
        public void Fail(Exception ex)
        {
            ULog.Error("{0}", ex.ToString());
            Outcome.FailMessage = ex.Message;
            Outcome.FailStackTrace = ex.StackTrace;
            Outcome.Result = Status.Failed;
        }

        /// <summary>
        /// Writes suite method status to log. Log level depends on execution status.
        /// </summary>
        protected void LogStatus()
        {
            string template = "{0} {1}";

            switch (Outcome.Result)
            {
                case Status.Failed:
                    ULog.Error(template, MethodType, Outcome.Result);
                    break;
                case Status.Skipped:
                    ULog.Warn(template, MethodType, Outcome.Result);
                    break;
                default:
                    ULog.Info(template, MethodType, Outcome.Result);
                    break;
            }
        }

        private void RunSuiteMethod(TestSuite testSuite)
        {
            try
            {
                var testTask = Task.Run(() =>
                {
                    TestMethod.Invoke(testSuite.SuiteInstance, null);
                });

                var restSuiteExecutionTime = Config.SuiteTimeout - testSuite.ExecutionTimer.Elapsed;

                if (restSuiteExecutionTime < TimeSpan.Zero)
                {
                    restSuiteExecutionTime = TimeSpan.Zero;
                }

                if (restSuiteExecutionTime <= Config.TestTimeout && !testTask.Wait(restSuiteExecutionTime))
                {
                    throw new SuiteTimeoutException($"Suite timeout ({Config.SuiteTimeout}) reached");
                }
                else if (!testTask.Wait(Config.TestTimeout))
                {
                    throw new TestTimeoutException($"{MethodType} timeout ({Config.TestTimeout}) reached");
                }

                Outcome.Result = Status.Passed;
                TafEvents.CallOnSuiteMethodPass(this);
            }
            catch (Exception ex)
            {
                var failExeption = ex is TestTimeoutException || ex is SuiteTimeoutException ?
                    ex :
                    ex.InnerException.InnerException;

                Fail(failExeption);
                TafEvents.CallOnSuiteMethodFail(this);
            }
        }
    }
}
