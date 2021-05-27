using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System;
using System.IO;
using WebApi.DAL.EF;
using WebApi.BLL.Interfaces;
using WebApi.BLL.Services;

namespace WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();

            services.AddDbContextPool<TestDbContext>(options =>
            {
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"),
                    connection =>
                    {
                        connection.CommandTimeout(Configuration.GetValue<int>("ConnectionStrings:CommandTimeout"));
                    })
                .UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()))
                .EnableSensitiveDataLogging();
            })
            .AddUnitOfWork<TestDbContext>();

            services.AddTransient<IExportService, CsvExportService>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "CSV Web Api"
                });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CSV Web Api v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
