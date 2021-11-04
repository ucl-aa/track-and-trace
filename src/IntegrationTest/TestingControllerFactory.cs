using System;
using System.Linq;
using Backend;
using Backend.Persistency;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace IntegrationTest
{
    public class TestingControllerFactory<T> : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                ServiceDescriptor? dbContext = 
                    services.SingleOrDefault(
                        d =>
                            d.ServiceType == typeof(DbContextOptions<TrackAndTraceContext>));
 
                if (dbContext != null)
                    services.Remove(dbContext);
 
                ServiceProvider serviceProvider = new ServiceCollection()
                    .AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();
 
                services.AddDbContext<TrackAndTraceContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryTrackAndTraceTest");
                    options.UseInternalServiceProvider(serviceProvider);
                });
                ServiceProvider? sp = services.BuildServiceProvider();

                using var scope = sp.CreateScope();
                using var appContext = scope.ServiceProvider.GetRequiredService<TrackAndTraceContext>();
                try
                {
                    appContext.Database.EnsureCreated();
                }
                catch (Exception ex)
                {
                    //Log errors
                    throw;
                }
            });
        }
    }
}