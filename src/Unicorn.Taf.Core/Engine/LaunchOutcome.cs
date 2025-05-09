﻿using System;
using System.Collections.Generic;
using System.Linq;
using Unicorn.Taf.Api;
using Unicorn.Taf.Core.Testing;

namespace Unicorn.Taf.Core.Engine
{
    /// <summary>
    /// Represents outcome of whole tests run. Contains run start time, list of test suites outcomes, overall run status.
    /// </summary>
    [Serializable]
    public class LaunchOutcome : IOutcome
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LaunchOutcome"/> class.
        /// </summary>
        public LaunchOutcome()
        {
            SuitesOutcomes = new List<SuiteOutcome>();
        }

        /// <summary>
        /// Gets or sets value of launch start time.
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Gets list of <see cref="SuiteOutcome"/> of the current run.
        /// </summary>
        public List<SuiteOutcome> SuitesOutcomes { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether launch was initialized without errors (assembly initialization was executed).
        /// </summary>
        public bool RunInitialized { get; set; } = true;

        /// <summary>
        /// Gets value indicating overall tests run status
        /// </summary>
        public Status RunStatus =>
            SuitesOutcomes
            .Any(o => o.Result.Equals(Status.Failed) || o.Result.Equals(Status.Skipped)) || !RunInitialized ?
            Status.Failed :
            Status.Passed;

        /// <summary>
        /// Gets or sets launch initialization fail exception message.
        /// </summary>
        public string InitFailMessage { get; set; }

        /// <summary>
        /// Gets or sets launch initialization fail exception stack trace.
        /// </summary>
        public string InitFailStackTrace { get; set; }
    }
}
