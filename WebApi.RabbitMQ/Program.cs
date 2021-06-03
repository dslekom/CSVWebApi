using Microsoft.Extensions.Hosting;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using WebApi.DAL.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using WebApi.DAL.Extensions;

namespace WebApi.RabbitMQ
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddDbContextPool<TestDbContext>(options =>
                    {
                        options.UseNpgsql(hostContext.Configuration.GetConnectionString("DefaultConnection"),
                            connection =>
                            {
                                connection.CommandTimeout(hostContext.Configuration.GetValue<int>("ConnectionStrings:CommandTimeout"));
                            })
                        .UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()))
                        .EnableSensitiveDataLogging();
                    })
                    .AddUnitOfWork<TestDbContext>();

                    services.AddMassTransit(busConfigurator =>
                    {
                        busConfigurator.AddConsumer<RemoveTestModelConsumer>();

                        busConfigurator.UsingRabbitMq((context, cfg) =>
                        {
                            cfg.ConfigureEndpoints(context);
                        });
                    });

                    services.AddMassTransitHostedService(true);
                });
    }
}
