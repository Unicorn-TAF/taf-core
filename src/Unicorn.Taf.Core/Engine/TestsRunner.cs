﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: https://pvs-studio.com
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Unicorn.Taf.Api;
using Unicorn.Taf.Core.Logging;
using Unicorn.Taf.Core.Testing;
using Unicorn.Taf.Core.Testing.Attributes;

namespace Unicorn.Taf.Core.Engine
{
    /// <summary>
    /// Provides ability to run tests which are filtered based on <see cref="Config"/>.
    /// </summary>
    public class TestsRunner : ITestRunner
    {
        private readonly Assembly _testAssembly;

        /// <summary>
        /// Initializes a new instance of the <see cref="TestsRunner"/> class for specified assembly.
        /// </summary>
        /// <param name="testAssembly">assembly with tests</param>
        /// <exception cref="FileNotFoundException">is thrown when tests assembly was not found</exception>
        public TestsRunner(Assembly testAssembly) : this(testAssembly, true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TestsRunner"/> class for specified assembly 
        /// based on specified configuration file.
        /// </summary>
        /// <param name="testAssembly">assembly with tests</param>
        /// <param name="configurationFileName">path to configuration file</param>
        /// <exception cref="FileNotFoundException">is thrown when tests assembly was not found</exception>
        public TestsRunner(Assembly testAssembly, string configurationFileName)
        {
            if (testAssembly == null)
            {
                throw new ArgumentNullException(nameof(testAssembly));
            }

            if (configurationFileName == null)
            {
                throw new ArgumentNullException(nameof(configurationFileName));
            }

            _testAssembly = testAssembly;
            Config.FillFromFile(configurationFileName);
            Outcome = new LaunchOutcome();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TestsRunner"/> class for specified assembly 
        /// with ability to specify if need to load file from default config.
        /// </summary>
        /// <param name="testAssembly">assembly with tests</param>
        /// <param name="getConfigFromFile">true - if need to load config from default file <c>(.\unicorn.conf)</c>; false if use default values from <see cref="Config"/></param>
        public TestsRunner(Assembly testAssembly, bool getConfigFromFile)
        {
            if (testAssembly == null)
            {
                throw new ArgumentNullException(nameof(testAssembly));
            }

            _testAssembly = testAssembly;

            if (getConfigFromFile)
            {
                Config.FillFromFile(string.Empty);
            }

            Outcome = new LaunchOutcome();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TestsRunner"/> class.
        /// </summary>
        protected TestsRunner()
        {
        }

        /// <summary>
        /// Gets or sets launch outcome
        /// </summary>
        public LaunchOutcome Outcome { get; protected set; }

        /// <summary>
        /// Run all observed tests matching selection criteria
        /// </summary>
        public virtual IOutcome RunTests()
        {
            ULog.Info("Scanning for runnable suites...");

            IEnumerable<Type> runnableSuites = TestsObserver.ObserveTestSuites(_testAssembly)
                .Where(s => AdapterUtilities.IsSuiteRunnable(s));

            if (!runnableSuites.Any())
            {
                ULog.Warn("No runnable suites found for specified filters, run finished.");
                return null;
            }

            ULog.Info("{0} runnable suites found, starting run...", runnableSuites.Count());

            Outcome.StartTime = DateTime.Now;

            // Execute run init action if exists in assembly.
            try
            {
                GetRunInitCleanupMethod(_testAssembly, typeof(RunInitializeAttribute))?.Invoke(null, null);
            }
            catch (Exception ex)
            {
                ULog.Error("Run initialization failed: {0}", ex);
                Outcome.RunInitialized = false;
                Outcome.RunnerException = ex.InnerException;
            }

            if (Outcome.RunInitialized)
            {
                ExecuteSuitesList(runnableSuites);

                // Execute run finalize action if exists in assembly.
                GetRunInitCleanupMethod(_testAssembly, typeof(RunFinalizeAttribute))?.Invoke(null, null);
            }

            ULog.Info("Run finished.");

            return Outcome;
        }

        /// <summary>
        /// Run test suite of specified <see cref="Type"/>
        /// </summary>
        /// <param name="type">suite <see cref="Type"/></param>
        protected void RunTestSuite(Type type)
        {
            if (AdapterUtilities.IsSuiteParameterized(type))
            {
                foreach (var parametersSet in AdapterUtilities.GetSuiteData(type))
                {
                    TestSuite parameterizedSuite = Activator
                        .CreateInstance(type, parametersSet.Parameters.ToArray()) as TestSuite;

                    parameterizedSuite.Metadata.Add("Data Set", parametersSet.Name);
                    parameterizedSuite.Outcome.DataSetName = parametersSet.Name;
                    ExecuteSuiteIteration(parameterizedSuite);
                }
            }
            else
            {
                TestSuite suite = Activator.CreateInstance(type) as TestSuite;
                ExecuteSuiteIteration(suite);
            }
        }

        /// <summary>
        /// Execute suite iteration (the suite itself for non-parameterized, suite instance for current data set)
        /// </summary>
        /// <param name="testSuite">suite instance</param>
        protected void ExecuteSuiteIteration(TestSuite testSuite)
        {
            testSuite.Execute();
            Outcome.SuitesOutcomes.Add(testSuite.Outcome);
        }

        /// <summary>
        /// Get <see cref="MethodInfo"/> representing run initialization / cleanup
        /// </summary>
        /// <param name="assembly">assembly to search within</param>
        /// <param name="attributeType">Type of attribute class should be marked with</param>
        /// <returns><see cref="MethodInfo"/> instance</returns>
        protected static MethodInfo GetRunInitCleanupMethod(Assembly assembly, Type attributeType)
        {
            IEnumerable<Type> suitesWithRunInit = assembly.GetTypes()
                .Where(t => t.IsDefined(typeof(TestAssemblyAttribute), true))
                .Where(s => GetStaticMethodsWithAttribute(s, attributeType).Any());

            return suitesWithRunInit.Any() ?
                GetStaticMethodsWithAttribute(suitesWithRunInit.First(), attributeType).First() :
                null;
        }

        private static IEnumerable<MethodInfo> GetStaticMethodsWithAttribute(Type containerType, Type attributeType) =>
            containerType.GetRuntimeMethods()
                .Where(m => m.IsDefined(attributeType, true));

        private void ExecuteSuitesList(IEnumerable<Type> suitesTypes)
        {
            if (Config.ParallelBy == Parallelization.Suite)
            {
                ParallelOptions parallelOptions = new ParallelOptions
                {
                    MaxDegreeOfParallelism = Config.Threads
                };

                Parallel.ForEach(suitesTypes, parallelOptions, suiteType => RunTestSuite(suiteType));
            }
            else
            {
                foreach (var suiteType in suitesTypes)
                {
                    RunTestSuite(suiteType);
                }
            }
        }
    }
}
