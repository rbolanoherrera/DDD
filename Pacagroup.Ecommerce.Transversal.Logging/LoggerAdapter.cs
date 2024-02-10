﻿using Microsoft.Extensions.Logging;
using Pacagroup.Ecommerce.Transversal.Common;

namespace Pacagroup.Ecommerce.Transversal.Logging
{
    public class LoggerAdapter<T> : IAppLogger<T>
    {
        private readonly ILogger<T> logger;

        public LoggerAdapter(ILoggerFactory loggerFactory)
        {
            this.logger = loggerFactory.CreateLogger<T>();
        }

        public void LogError(string message)
        {
            logger.LogError(message);
            LoggerText.writeLog(message);
        }

        public void LogError(string message, params object[] args)
        {
            logger.LogError(message, args);
            LoggerText.writeLog(message);
        }

        public void LogInformation(string message)
        {
            logger.LogInformation(message);
            LoggerText.writeLog(message);
        }

        public void LogInformation(string message, params object[] args)
        {
            logger.LogInformation(message, args);
            LoggerText.writeLog(message);
        }

        public void LogWarning(string message)
        {
            logger.LogWarning(message);
            LoggerText.writeLog(message);
        }

        public void LogWarning(string message, params object[] args)
        {
            logger.LogWarning(message, args);
            LoggerText.writeLog(message);
        }
    }
}
