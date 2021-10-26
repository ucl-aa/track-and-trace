using System;
using Microsoft.Extensions.Logging;

namespace Backend.Loggers
{
    public interface IExceptionLogger
    {
        void LogException(Exception exception, string controllerName, ILogger logger);
    }
}