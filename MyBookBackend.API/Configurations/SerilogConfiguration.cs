using Serilog;
using Serilog.Events;

namespace MyBookBackend.API.Configurations;

public static class SerilogConfiguration
{
    public static ConfigureHostBuilder AddSerilogConfiguration(
        this ConfigureHostBuilder host)
    {
        Log.Logger = new LoggerConfiguration()

            .MinimumLevel.Information()

            .MinimumLevel.Override(
                "Microsoft",
                LogEventLevel.Warning)

            .MinimumLevel.Override(
                "Microsoft.AspNetCore",
                LogEventLevel.Warning)

            .WriteTo.Console()

            .WriteTo.File(
                "Logs/log-.txt",
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 7,
                shared: true)

            .CreateLogger();

        host.UseSerilog();

        return host;
    }
}