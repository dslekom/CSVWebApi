using MassTransit;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using WebApi.Common.RabbitMQ;

namespace WebApi.RabbitMq.Client
{
    public class DeleteTestModelWorker : BackgroundService
    {
        readonly IBus bus;

        public DeleteTestModelWorker(IBus bus)
        {
            this.bus = bus;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                Console.WriteLine("Enter test model ID (or quit to exit)");
                Console.Write("> ");
                string value = Console.ReadLine();

                if ("quit".Equals(value, StringComparison.OrdinalIgnoreCase))
                    break;

                if (int.TryParse(value, out int id))
                {
                    await bus.Publish(new RemoveTestModel { ID = id });
                }
            }
        }
    }
}
