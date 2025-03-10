using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using System;
using Serilog;
using Serilog.Events;
using Microsoft.Extensions.Hosting;

namespace Sporadic.Abp.Identity.HttpApi.Host
{
    public class Program
    {
        public async static Task<int> Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
#if DEBUG
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Information)
                .WriteTo.Async(c => c.Console())
                .WriteTo.Async(c => c.File(
                    "logs/debug.txt",
                    rollingInterval: RollingInterval.Day,
                    rollOnFileSizeLimit: true,
                    fileSizeLimitBytes: 5 * 1024 * 1024))
#else
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
                .WriteTo.Async(c => c.File(
                    "logs/logs.txt",
                    rollingInterval: RollingInterval.Day,
                    rollOnFileSizeLimit: true,
                    fileSizeLimitBytes: 5 * 1024 * 1024))
#endif
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .CreateLogger();
            try
            {
                Log.Information("Starting CrmKit Host");
                var builder = WebApplication.CreateBuilder(args);
                builder.Host.UseAutofac().UseSerilog();
                await builder.AddApplicationAsync<SporadicIdentityHttpApiHostModule>();
                var app = builder.Build();
                await app.InitializeApplicationAsync();
                await app.RunAsync();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly!");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
