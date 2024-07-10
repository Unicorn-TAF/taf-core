// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: https://pvs-studio.com
namespace Unicorn.Taf.Core.Testing
{
    /// <summary>
    /// Represents execution status
    /// </summary>
    public enum Status
    {
        /// <summary>
        /// Execution is completed successfully.
        /// </summary>
        Passed,

        /// <summary>
        /// Execution is failed.
        /// </summary>
        Failed,

        /// <summary>
        /// Execution was planned but skipped.
        /// </summary>
        Skipped,

        /// <summary>
        /// Execution was not planned.
        /// </summary>
        NotExecuted
    }
}
