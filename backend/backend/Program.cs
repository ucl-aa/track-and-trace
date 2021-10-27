using System;
using Backend.Persistency;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Backend
{
    public class Program
    {
        private static IHost _host;

        public static void Main(string[] args)
        {
            _host = CreateHostBuilder(args).Build();

            SetupDbContext();

            _host.Run();
        }

        private static void SetupDbContext()
        {
            IServiceProvider serviceProvider = GetServiceProvider();

            try
            {
                SetupTrackAndTraceContext(serviceProvider.GetRequiredService<TrackAndTraceContext>());
            }
            catch (Exception exception)
            {
                LogException(exception, serviceProvider.GetRequiredService<ILogger<Program>>());
            }
        }

        private static IServiceProvider GetServiceProvider()
        {
            IServiceScope scope = _host.Services.CreateScope();

            return scope.ServiceProvider;
        }

        private static void SetupTrackAndTraceContext(TrackAndTraceContext context)
        {
            DatabaseFacade facade = context.Database;

            facade.Migrate();
            facade.EnsureCreated();
        }

        private static void LogException(Exception exception, ILogger<Program> logger)
        {
            logger.LogError(exception, "An error occurred creating the DB: ");
        }


        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}