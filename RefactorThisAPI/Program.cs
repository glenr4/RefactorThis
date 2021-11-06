using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using RefactorThis.API.Logging;
using Serilog;
using Serilog.Events;

namespace RefactorThisAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
               .MinimumLevel.Debug()
               .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
               .MinimumLevel.Override("System", LogEventLevel.Warning)
               .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Warning)
               .Enrich.FromLogContext()
               .Enrich.With<ActivityEnricher>()
               .WriteTo.File(
                    "../logs/webapp.log",
                    rollingInterval: RollingInterval.Day,
                    rollOnFileSizeLimit: true,
                    outputTemplate: "[{Timestamp:HH:mm:ss.fff} {Level}] {SourceContext} {Message:lj} {Exception}{NewLine}")
               .CreateLogger();

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                }).UseSerilog();
    }
}