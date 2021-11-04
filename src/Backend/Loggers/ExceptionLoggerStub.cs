using System;
using Microsoft.Extensions.Logging;

namespace Backend.Loggers
{
    public class ExceptionLoggerStub : IExceptionLogger
    {
        public void Log(Exception exception, string controllerName, ILogger logger)
        {
        }
    }
}