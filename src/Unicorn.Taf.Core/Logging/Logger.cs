// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: https://pvs-studio.com
namespace Unicorn.Taf.Core.Logging
{
    /// <summary>
    /// Entry point of framework logger.
    /// </summary>
    public static class Logger
    {
        private static ILogger _instance;

        /// <summary>
        /// Gets or sets framework logger instance (default logger is <see cref="DefaultLogger"/>).
        /// </summary>
        public static ILogger Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DefaultLogger();
                    _instance.Log(LogLevel.Info, "Default console logger is initialized");
                }
                    
                return _instance;
            }

            set
            {
                _instance = value;
            }
        }

        /// <summary>
        /// Gets or sets value to filter out log messages by severity (default is Debug).
        /// </summary>
        public static LogLevel Level { get; set; } = LogLevel.Debug;
    }
}
