using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WebApi.DAL.EF;
using WebApi.DAL.Extensions;
using WebApi.gRPC.Services;

namespace WebApi.gRPC
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
            services.AddGrpc();

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
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<TestModelService>();
            });
        }
    }
}
