using Microsoft.Extensions.Hosting;

using Serilog;
using Serilog.Events;

namespace PoeCurrencyIndexer.Host.Configuration
{
    public static class LoggingExtensions
    {
        public static IHostBuilder ConfigureLogging(
            this IHostBuilder hostBuilder) => hostBuilder.UseSerilog((_, conf) =>
                conf
                    .MinimumLevel.Debug()
                    .WriteTo.Console(
                        LogEventLevel.Verbose,
                        "{Timestamp:HH:mm:ss.fffff} [{Level:u3}] [{ThreadID}] {Scope,-15}" +
                        "{Message:lj}{NewLine}{Exception}",
                        theme: Serilog.Sinks.SystemConsole.Themes.AnsiConsoleTheme.Code)
                    .Enrich.FromLogContext()
                    .Enrich.With<ThreadIdEnricher>());
    }
}