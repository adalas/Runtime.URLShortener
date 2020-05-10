using Runtime.URLShortener.ApplicationCore.Interfaces;
using Microsoft.Extensions.Logging;
using System;
namespace Runtime.URLShortener.Infrastructure.Logging
{
    public class LoggerAdapter<T> : IAppLogger<T>
    {
        private readonly ILogger<T> _logger;
        public LoggerAdapter(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<T>();
        }

        public void LogWarning(string message, params object[] args)
        {
            _logger.LogWarning(message, args);
        }

        public void LogInformation(string message, params object[] args)
        {
            _logger.LogInformation(message, args);
        }

        public void LogError(Exception exception, string message)
        {
            _logger.LogError(exception,message);
        }
    }
}
