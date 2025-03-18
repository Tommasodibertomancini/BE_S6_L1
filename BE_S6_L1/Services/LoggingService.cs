using Serilog.Events;
using Serilog;

namespace BE_S6_L1.Services
{
    public static class LoggingService
    {
        public static void ConfigureLogging(IConfiguration configuration)
        {
           
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                .WriteTo.Console()
                .WriteTo.File("logs/studentiapp-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            Log.Information("Configurazione di logging completata");
        }
    }
}
