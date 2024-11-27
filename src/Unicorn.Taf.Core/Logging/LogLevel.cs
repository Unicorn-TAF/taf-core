namespace Unicorn.Taf.Core.Logging
{
    /// <summary>
    /// Represents severity levels for framework logger.
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// Used to log errors which are not handled.
        /// </summary>
        Error,

        /// <summary>
        /// Used to log errors which are handled.
        /// </summary>
        Warning,

        /// <summary>
        /// Used to log general information.
        /// </summary>
        Info,

        /// <summary>
        /// Used to log information for debugging.
        /// </summary>
        Debug,

        /// <summary>
        /// Used to log low level information.
        /// </summary>
        Trace
    }
}
