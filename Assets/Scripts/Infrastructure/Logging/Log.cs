using System.Collections.Generic;

namespace Assets.Scripts.Infrastructure.Logging
{
    internal static class Log
    {
        private static readonly IList<ILogger> _loggers = new List<ILogger>();
        
        public static void AddLogger(ILogger logger)
        {
            if(logger!=null)
                _loggers.Add(logger);
        }

        public static void RemoveLogger(ILogger logger)
        {
            _loggers.Remove(logger);
        }

        public static void Error(object message, string tag = "")
        {
            LogMessage(LogType.Error, message, tag);
        }

        public static void Warning(object message, string tag = "")
        {
            LogMessage(LogType.Warning, message, tag);
        }

        public static void Verbose(object message, string tag = "")
        {
            LogMessage(LogType.Verbose, message, tag);
        }

        private static void LogMessage(LogType type, object message, string tag = "")
        {
            foreach (var logger in _loggers)
            {
                logger.Log(type, message, tag);
            }
        }
    }
}
