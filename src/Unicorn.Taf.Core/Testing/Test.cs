using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Unicorn.Taf.Core.Logging;
using Unicorn.Taf.Core.Testing.Attributes;

namespace Unicorn.Taf.Core.Testing
{
    /// <summary>
    /// Represents the test itself.
    /// Contains list of events related to different Test states (started, finished, skipped, passed, failed)<para/>
    /// Contains methods to execute the test and check if test should be skipped<para/>
    /// </summary>
    public class Test : SuiteMethod
    {
        private readonly DataSet _dataSet;

        private HashSet<string> categories = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="Test"/> class
        /// based on actual <see cref="MethodInfo"/> with test body
        /// </summary>
        /// <param name="testMethod"><see cref="MethodInfo"/> instance which represents test method</param>
        public Test(MethodInfo testMethod) : base(testMethod)
        {
            _dataSet = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Test"/> class 
        /// based on actual <see cref="MethodInfo"/> with test body and specified <see cref="DataSet"/>.
        /// </summary>
        /// <param name="testMethod"><see cref="MethodInfo"/> instance which represents test method</param>
        /// <param name="dataSet"><see cref="DataSet"/> to populate test method parameters; null if method does not have parameters</param>
        public Test(MethodInfo testMethod, DataSet dataSet) : base(testMethod)
        {
            var postfix = $" [{dataSet.Name}]";
            _dataSet = dataSet;

            Outcome.Title += postfix;
        }

        /// <summary>
        /// Gets test categories
        /// </summary>
        public HashSet<string> Categories
        {
            get
            {
                if (categories == null)
                {
                    var attributes = TestMethod.GetCustomAttributes<CategoryAttribute>(true);

                    categories = new HashSet<string>(
                        attributes.Select(a => a.Category.ToUpper().Trim())
                        .Where(c => !string.IsNullOrEmpty(c)));
                }

                return categories;
            }
        }

        /// <summary>
        /// Execute current test and fill <see cref="TestOutcome"/> <para/>
        /// Before the test List of BeforeTests is executed <para/>
        /// After the test List of AfterTests is executed <para/>
        /// </summary>
        /// <param name="suiteInstance">test suite instance to run in</param>
        public override void Execute(TestSuite suiteInstance)
        {
            ULog.Info("-------- Test '{0}'", Outcome.Title);

            TafEvents.CallOnTestStart(this);

            Outcome.StartTime = DateTime.Now;
            TestTimer = Stopwatch.StartNew();

            RunTestMethod(suiteInstance);

            TestTimer.Stop();
            Outcome.ExecutionTime = TestTimer.Elapsed;
            LogStatus();

            TafEvents.CallOnTestFinish(this);
        }

        /// <summary>
        /// Skip test and invoke OnTestSkip event
        /// </summary>
        public void Skip(string reason)
        {
            Outcome.Result = Status.Skipped;
            Outcome.StartTime = DateTime.Now;
            Outcome.ExecutionTime = TimeSpan.FromSeconds(0);
            Outcome.FailMessage = reason;
            ULog.Warn("Test '{0}' {1}", Outcome.Title, Outcome.Result);
            TafEvents.CallOnTestSkip(this);
        }

        private void RunTestMethod(TestSuite testSuite)
        {
            try
            {
                var testTask = Task.Run(() =>
                {
                    TestMethod.Invoke(testSuite.SuiteInstance, _dataSet?.Parameters.ToArray());
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
                    throw new TestTimeoutException($"Test timeout ({Config.TestTimeout}) reached");
                }

                Outcome.Result = Status.Passed;
                TafEvents.CallOnTestPass(this);
            }
            catch (Exception ex)
            {
                var failExeption = ex is TestTimeoutException || ex is SuiteTimeoutException ?
                    ex :
                    ex.InnerException.InnerException;

                Fail(failExeption);
                TafEvents.CallOnTestFail(this);
            }
        }
    }
}
