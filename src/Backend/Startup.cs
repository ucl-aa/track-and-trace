using Backend.Loggers;
using Backend.Persistency;
using Backend.Services;
using Backend.Services.Generics;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Backend
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddScoped<IZipCodeService, ZipCodeService>();
            services.AddScoped<IStatusService, StatusService>();
            services.AddScoped(typeof(AddService<>), typeof(AddService<>));
            services.AddScoped(typeof(GetService<>), typeof(GetService<>));
            services.AddScoped(typeof(DeleteService<>), typeof(DeleteService<>));

            services.AddScoped<IExceptionLogger, ExceptionLoggerStub>();
            services
                .AddSwaggerGen(
                    c => { c.SwaggerDoc("v1", new OpenApiInfo { Title = "backend", Version = "v1" }); });
            services.AddDbContext<TrackAndTraceContext>(
                options => options.UseSqlite(
                    Configuration.GetConnectionString("TrackAndTraceContext")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "backend v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}