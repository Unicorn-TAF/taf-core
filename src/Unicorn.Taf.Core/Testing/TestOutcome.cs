﻿using System;
using System.Collections.Generic;

namespace Unicorn.Taf.Core.Testing
{
    /// <summary>
    /// Represents outcome of executed <see cref="Test"/> or <see cref="SuiteMethod"/>. Contains next info:<para/>
    /// Required info: ID; Suite ID; Title; Full method name; Author; Execution result; Start time; Duration;<para/>
    /// Optional info (if failed): Exception; Screenshot; Defect reference
    /// </summary>
    [Serializable]
    public class TestOutcome
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestOutcome"/> class.
        /// </summary>
        public TestOutcome()
        {
            ExecutionTime = TimeSpan.FromSeconds(0);
            Attachments = new List<Attachment>();
        }

        /// <summary>
        /// Gets or sets <see cref="Guid"/> of test (unique across test assembly).
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets parent suite <see cref="Guid"/> (unique across test assembly).
        /// </summary>
        public Guid ParentId { get; set; }

        /// <summary>
        /// Gets or sets ID of tied test case.
        /// </summary>
        public string TestCaseId { get; set; }

        /// <summary>
        /// Gets or sets test title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets Full test method name including declaring class and it's namespace.
        /// </summary>
        public string FullMethodName { get; set; }

        /// <summary>
        /// Gets or sets test author.
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// Gets or sets Execution Result.
        /// </summary>
        public Status Result { get; set; }

        /// <summary>
        /// Gets or sets Test start time as DateTime.
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Gets or sets Test execution time as TimeSpan.
        /// </summary>
        public TimeSpan ExecutionTime { get; set; }

        /// <summary>
        /// Gets or sets fail exception message.
        /// </summary>
        public string FailMessage { get; set; }

        /// <summary>
        /// Gets or sets fail exception stack trace.
        /// </summary>
        public string FailStackTrace { get; set; }

        /// <summary>
        /// Gets or sets list of test attachments.
        /// </summary>
        public List<Attachment> Attachments { get; set; }

        /// <summary>
        /// Gets or sets Array of bugs attached to the test. Has values only when the test failed by bug.
        /// </summary>
        public Defect Defect { get; set; }

        /// <summary>
        /// Gets or sets test log output string
        /// </summary>
        public string Output { get; set; }

        /// <summary>
        /// Adds new line with specified text to test output.
        /// </summary>
        /// <param name="text">text to add</param>
        public void AppendOutput(string text) =>
            Output += text;
    }
}
