using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Exceptions;
using Serilog.Formatting.Json;
using Serilog.Sinks.SystemConsole.Themes;
using Throw;

using ILogger = Serilog.ILogger;

namespace SourceName.Api.Loaders;

internal static class LoggingConfiguration
{
    internal static ILogger AddLogging(this WebApplicationBuilder builder)
    {
        builder.Logging.ClearProviders();

#if EnableFileLogger
		var loggingFilePath = builder.Configuration.GetSection("LoggingFilePath").Value;
		loggingFilePath.ThrowIfNull().IfNullOrWhiteSpace(_ => "LoggingFilePath",
		    "When using file logger for serilog, LoggingFilePath must be set in appsettings or environment configuration");
#endif

        ILogger logger = new LoggerConfiguration()
            .Enrich.WithExceptionDetails()
            .Enrich.WithDemystifiedStackTraces()
            .WriteTo.Console(theme: AnsiConsoleTheme.Code)
#if EnableFileLogger
            .WriteTo.File(new JsonFormatter(renderMessage: true), loggingFilePath, rollingInterval: RollingInterval.Day)
#endif
            .CreateLogger();

        builder.Host.UseSerilog(logger);
        return logger;
    }
}