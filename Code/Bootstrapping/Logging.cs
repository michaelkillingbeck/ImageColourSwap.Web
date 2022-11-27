using AWS.Logger;
using Microsoft.Extensions.Configuration;

namespace Web.Bootstrapping;

internal class LoggingBootstrapping
{
    internal static void AddLogging(WebApplicationBuilder builder)
    {
        if (builder.Environment.EnvironmentName == "Development")
        {
            var loggingConfig = builder.Configuration.GetAWSLoggingConfigSection("Logging-Development");

            builder.Services.AddLogging(
                config => 
                { 
                    config.AddAWSProvider(loggingConfig); 
                    config.SetMinimumLevel(LogLevel.Debug); 
                });
        }
        else
        {
            var loggingSection = builder.Configuration.GetSection("Logging");
            var awsLoggingSection = new AWSLoggerConfigSection(loggingSection);
            awsLoggingSection.Config.Region = "eu-west-2";

            builder.Services.AddLogging(
                config => 
                { 
                    config.AddAWSProvider(awsLoggingSection); 
                    config.SetMinimumLevel(LogLevel.Information); 
                });
        }
    }
}