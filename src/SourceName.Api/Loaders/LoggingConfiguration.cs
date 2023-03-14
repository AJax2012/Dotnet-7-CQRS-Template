using Serilog;
using Serilog.Exceptions;

using ILogger = Serilog.ILogger;

namespace SourceName.Api.Loaders;

internal static class LoggingConfiguration
{
    internal static ILogger AddLogging(this WebApplicationBuilder builder)
    {
        builder.Logging.ClearProviders();

#if EnableFileLogger
var loggingFilePath = builder.Configuration.GetSection("LoggingFilePath").Value;
Guard.Against.NullOrWhiteSpace(loggingFilePath, "LoggingFilePath",
    "When using file logger for serilog, LoggingFilePath must be set in appsettings or environment configuration");
#endif

        ILogger logger = new LoggerConfiguration()
            .Enrich.WithExceptionDetails()
            .Enrich.WithDemystifiedStackTraces()
            .WriteTo.Console()
#if EnableFileLogger
    .WriteTo.File(new JsonFormatter(renderMessage: true), loggingFilePath, rollingInterval: RollingInterval.Day)
#endif
            .CreateLogger();

        builder.Host.UseSerilog(logger);
        return logger;
    }
}