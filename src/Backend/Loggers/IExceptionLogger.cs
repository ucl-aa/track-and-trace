using System;
using Microsoft.Extensions.Logging;

namespace Backend.Loggers
{
    public interface IExceptionLogger
    {
        void Log(Exception exception, string controllerName, ILogger logger);
    }
}