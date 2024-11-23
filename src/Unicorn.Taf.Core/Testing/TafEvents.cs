using System;
using System.Diagnostics.CodeAnalysis;
using Unicorn.Taf.Core.Logging;

namespace Unicorn.Taf.Core.Testing
{
    /// <summary>
    /// Entry point for framework events.
    /// </summary>
    public static class TafEvents
    {
        #region Suite events

        /// <summary>
        /// Event is invoked before suite execution
        /// </summary>
        public static event UnicornSuiteEvent OnSuiteStart;

        /// <summary>
        /// Event is invoked after suite execution
        /// </summary>
        public static event UnicornSuiteEvent OnSuiteFinish;

        /// <summary>
        /// Event is invoked if suite is skipped
        /// </summary>
        public static event UnicornSuiteEvent OnSuiteSkip;

        #endregion

        #region Suite Method events

        /// <summary>
        /// Event is invoked before suite method execution
        /// </summary>
        public static event UnicornSuiteMethodEvent OnSuiteMethodStart;

        /// <summary>
        /// Event is invoked after suite method execution
        /// </summary>
        public static event UnicornSuiteMethodEvent OnSuiteMethodFinish;

        /// <summary>
        /// Event is invoked on suite method pass (<see cref="OnSuiteMethodFinish"/> OnTestFinish will be invoked anyway)
        /// </summary>
        public static event UnicornSuiteMethodEvent OnSuiteMethodPass;

        /// <summary>
        /// Event is invoked on suite method fail (<see cref="OnSuiteMethodFinish"/> will be invoked anyway)
        /// </summary>
        public static event UnicornSuiteMethodEvent OnSuiteMethodFail;

        #endregion

        #region Test events

        /// <summary>
        /// Event is invoked before test execution
        /// </summary>
        public static event TestEvent OnTestStart;

        /// <summary>
        /// Event is invoked after test execution
        /// </summary>
        public static event TestEvent OnTestFinish;

        /// <summary>
        /// Event is invoked on test pass (<see cref="OnTestFinish"/> will be invoked anyway)
        /// </summary>
        public static event TestEvent OnTestPass;

        /// <summary>
        /// Event is invoked on test fail (<see cref="OnTestFinish"/> will be invoked anyway)
        /// </summary>
        public static event TestEvent OnTestFail;

        /// <summary>
        /// Event is invoked on test skip
        /// </summary>
        public static event TestEvent OnTestSkip;

        #endregion

        /// <summary>
        /// Delegate used for suite events invocation
        /// </summary>
        /// <param name="testSuite">current <see cref="TestSuite"/> instance</param>
        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
        public delegate void UnicornSuiteEvent(TestSuite testSuite);

        /// <summary>
        /// Delegate used for suite method events invocation
        /// </summary>
        /// <param name="suiteMethod">current <see cref="SuiteMethod"/> instance</param>
        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
        public delegate void UnicornSuiteMethodEvent(SuiteMethod suiteMethod);

        /// <summary>
        /// Delegate used for test events invocation
        /// </summary>
        /// <param name="test">current <see cref="Test"/> instance</param>
        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
        public delegate void TestEvent(Test test);

        internal static void CallOnSuiteStart(TestSuite suite) =>
            ExecuteSuiteEvent(OnSuiteStart, suite, nameof(OnSuiteStart));

        internal static void CallOnSuiteFinish(TestSuite suite) =>
            ExecuteSuiteEvent(OnSuiteFinish, suite, nameof(OnSuiteFinish));

        internal static void CallOnSuiteSkip(TestSuite suite) =>
            ExecuteSuiteEvent(OnSuiteSkip, suite, nameof(OnSuiteSkip));


        internal static void CallOnSuiteMethodStart(SuiteMethod suiteMethod) =>
            ExecuteSuiteMethodEvent(OnSuiteMethodStart, suiteMethod, nameof(OnSuiteMethodStart));

        internal static void CallOnSuiteMethodFinish(SuiteMethod suiteMethod) =>
            ExecuteSuiteMethodEvent(OnSuiteMethodFinish, suiteMethod, nameof(OnSuiteMethodFinish));

        internal static void CallOnSuiteMethodPass(SuiteMethod suiteMethod) =>
            ExecuteSuiteMethodEvent(OnSuiteMethodPass, suiteMethod, nameof(OnSuiteMethodPass));

        internal static void CallOnSuiteMethodFail(SuiteMethod suiteMethod) =>
            ExecuteSuiteMethodEvent(OnSuiteMethodFail, suiteMethod, nameof(OnSuiteMethodFail));


        internal static void CallOnTestStart(Test test) =>
            ExecuteTestEvent(OnTestStart, test, nameof(OnTestStart));

        internal static void CallOnTestFinish(Test test) =>
            ExecuteTestEvent(OnTestFinish, test, nameof(OnTestFinish));

        internal static void CallOnTestSkip(Test test) =>
            ExecuteTestEvent(OnTestSkip, test, nameof(OnTestSkip));

        internal static void CallOnTestPass(Test test) =>
            ExecuteTestEvent(OnTestPass, test, nameof(OnTestPass));

        internal static void CallOnTestFail(Test test) =>
            ExecuteTestEvent(OnTestFail, test, nameof(OnTestFail));

        private static void ExecuteSuiteEvent(UnicornSuiteEvent e, TestSuite suite, string eventName)
        {
            try
            {
                e?.Invoke(suite);
            }
            catch (Exception ex)
            {
                LogEventCallError(eventName, ex.ToString());
            }
        }

        private static void ExecuteSuiteMethodEvent(UnicornSuiteMethodEvent e, SuiteMethod suiteMethod, string eventName)
        {
            try
            {
                e?.Invoke(suiteMethod);
            }
            catch (Exception ex)
            {
                LogEventCallError(eventName, ex.ToString());
            }
        }

        private static void ExecuteTestEvent(TestEvent e, Test test, string eventName)
        {
            try
            {
                e?.Invoke(test);
            }
            catch (Exception ex)
            {
                LogEventCallError(eventName, ex.ToString());
            }
        }

        private static void LogEventCallError(string eventName, string error) =>
            ULog.Warn("Exception occured during '{0}' event call: {1}", eventName, error);
    }
}
