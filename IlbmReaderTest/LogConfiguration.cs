using System;
using Autofac;
using Serilog;

namespace IlbmReaderTest
{
    public class LogConfiguration
    {
        public static ILogger Create()
        {
            var logger = new LoggerConfiguration()
                .WriteTo.ColoredConsole()
                .WriteTo.RollingFile("log-{Date}.txt", fileSizeLimitBytes: null, retainedFileCountLimit: null)
                .CreateLogger();

            return logger;
        }
    }
}