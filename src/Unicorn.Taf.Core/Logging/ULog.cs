using Unicorn.Taf.Api;

namespace Unicorn.Taf.Core.Logging
{
    /// <summary>
    /// Main framework logger.
    /// Defaults: level - Debug, output - console.
    /// </summary>
    public static class ULog
    {
        private static LogLevel Level = LogLevel.Debug;
        private static ILogger Instance = new DefaultConsoleLogger();

        /// <summary>
        /// Sets active logger implementation.
        /// </summary>
        /// <param name="logger">logger instance</param>
        public static void SetLogger(Api.ILogger logger) =>
            Instance = logger;

        /// <summary>
        /// Sets minimum log level. All records with level lower than current will not appear in logs.
        /// </summary>
        /// <param name="level">verbosity level</param>
        public static void SetLevel(LogLevel level) =>
            Level = level;

        /// <summary>
        /// Logs message with error level.
        /// </summary>
        /// <param name="message">message to log</param>
        /// <param name="parameters">parameters to substitute to message template</param>
        public static void Error(string message, params object[] parameters)
        {
            Instance.Error(message, parameters);
        }

        /// <summary>
        /// Logs message with warning level.
        /// </summary>
        /// <param name="message">message to log</param>
        /// <param name="parameters">parameters to substitute to message template</param>
        public static void Warn(string message, params object[] parameters)
        {
            if (Level >= LogLevel.Warning)
            {
                Instance.Warn(message, parameters);
            }
        }

        /// <summary>
        /// Logs message with informational level.
        /// </summary>
        /// <param name="message">message to log</param>
        /// <param name="parameters">parameters to substitute to message template</param>
        public static void Info(string message, params object[] parameters)
        {
            if (Level >= LogLevel.Info)
            {
                Instance.Info(message, parameters);
            }
        }

        /// <summary>
        /// Logs message with debug level.
        /// </summary>
        /// <param name="message">message to log</param>
        /// <param name="parameters">parameters to substitute to message template</param>
        public static void Debug(string message, params object[] parameters)
        {
            if (Level >= LogLevel.Debug)
            {
                Instance.Debug(message, parameters);
            }
        }

        /// <summary>
        /// Logs message with trace level.
        /// </summary>
        /// <param name="message">message to log</param>
        /// <param name="parameters">parameters to substitute to message template</param>
        public static void Trace(string message, params object[] parameters)
        {
            if (Level >= LogLevel.Trace)
            {
                Instance.Trace(message, parameters);
            }
        }
    }
}
