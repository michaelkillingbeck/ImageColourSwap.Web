namespace Web.Bootstrapping;

internal static class LoggingBootstrapping
{
    private static readonly Action<ILogger, Exception, Exception?> _logException =
        LoggerMessage.Define<Exception>(
            logLevel: LogLevel.Error,
            eventId: 1,
            formatString: "Exception caught: {ExceptionMessage}");

    private static readonly Action<ILogger, string, Exception?> _logInformation =
        LoggerMessage.Define<string>(
            logLevel: LogLevel.Information,
            eventId: 1,
            formatString: "{Message}");

    internal static void AddLogging(WebApplicationBuilder builder)
    {
        if (builder.Environment.EnvironmentName == "Development")
        {
            AWSLoggerConfigSection loggingConfig = builder.Configuration.GetAWSLoggingConfigSection("Logging-Development");

            _ = builder.Services.AddLogging(
                config =>
                {
                    _ = config.AddAWSProvider(loggingConfig);
                    _ = config.SetMinimumLevel(LogLevel.Debug);
                });
        }
        else
        {
            IConfigurationSection loggingSection = builder.Configuration.GetSection("Logging");
            AWSLoggerConfigSection awsLoggingSection = new(loggingSection);
            awsLoggingSection.Config.Region = "eu-west-2";

            _ = builder.Services.AddLogging(
                config =>
                {
                    _ = config.AddAWSProvider(awsLoggingSection);
                    _ = config.SetMinimumLevel(LogLevel.Information);
                });
        }
    }

    internal static void LogException(this ILogger logger, Exception exception)
    {
        _logException(logger, exception, null);
    }

    internal static void LogInformationMessage(this ILogger logger, string message)
    {
        _logInformation(logger, message, null);
    }
}