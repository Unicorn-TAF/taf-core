using System;
using System.Globalization;
using Unicorn.Taf.Api;

namespace Unicorn.Taf.Core.Logging
{
    /// <summary>
    /// Provides default implementation of framework logger (used if no other loggers were assigned).
    /// Output: console.
    /// </summary>
    public class DefaultConsoleLogger : ILogger
    {
        private const string DtFormat = "yyyy/MM/dd HH:mm:ss.ff";

        private const string ErrorPrefix = "  [Error]: ";
        private const string WarningPrefix = "[Warning]: ";
        private const string InfoPrefix = "   [Info]: ";
        private const string DebugPrefix = "  [Debug]:   ";
        private const string TracePrefix = "  [Trace]:     ";

        /// <summary>
        /// Initializes a new instance of Console logger.
        /// </summary>
        public DefaultConsoleLogger()
        {
            Console.WriteLine("Default console logger has been initialized");
        }

        /// <summary>
        /// Logs message with error level.
        /// </summary>
        /// <param name="message">message to log</param>
        /// <param name="parameters">parameters to substitute to message template</param>
        public void Error(string message, params object[] parameters) =>
            Log(ErrorPrefix, message, parameters);

        /// <summary>
        /// Logs message with warning level.
        /// </summary>
        /// <param name="message">message to log</param>
        /// <param name="parameters">parameters to substitute to message template</param>
        public void Warn(string message, params object[] parameters) =>
            Log(WarningPrefix, message, parameters);

        /// <summary>
        /// Logs message with informational level.
        /// </summary>
        /// <param name="message">message to log</param>
        /// <param name="parameters">parameters to substitute to message template</param>
        public void Info(string message, params object[] parameters) =>
            Log(InfoPrefix, message, parameters);

        /// <summary>
        /// Logs message with debug level.
        /// </summary>
        /// <param name="message">message to log</param>
        /// <param name="parameters">parameters to substitute to message template</param>
        public void Debug(string message, params object[] parameters) =>
            Log(DebugPrefix, message, parameters);

        /// <summary>
        /// Logs message with trace level.
        /// </summary>
        /// <param name="message">message to log</param>
        /// <param name="parameters">parameters to substitute to message template</param>
        public void Trace(string message, params object[] parameters) =>
            Log(TracePrefix, message, parameters);

        private void Log(string prefix, string message, object[] parameters)
        {
            string timestamp = DateTime.Now.ToString(DtFormat, CultureInfo.InvariantCulture);

            if (parameters.Length == 0)
            {
                Console.WriteLine($"{timestamp} {prefix}{message}");
            }
            else
            {
                Console.WriteLine($"{timestamp} {prefix}{string.Format(message, parameters)}");
            }
        }
    }
}
