using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using WebApi.Common.RabbitMQ;
using WebApi.DAL.Entities;
using WebApi.DAL.Interfaces;

namespace WebApi.RabbitMQ
{
    public class RemoveTestModelConsumer : IConsumer<RemoveTestModel>
    {
        private readonly ILogger<RemoveTestModelConsumer> logger;
        private readonly IUnitOfWork database;

        public RemoveTestModelConsumer(ILogger<RemoveTestModelConsumer> logger, IUnitOfWork database)
        {
            this.logger = logger;
            this.database = database;
        }

        public async Task Consume(ConsumeContext<RemoveTestModel> context)
        {
            logger.LogInformation($"Received ID: {context.Message.ID}");

            var testModel = await database.GetRepository<TestModel>().GetAsync(context.Message.ID);

            if (testModel != null)
            {
                testModel.IsDeleted = true;
                testModel.EditedAt = DateTime.Now;

                database.GetRepository<TestModel>().Update(testModel);
                await database.SaveAsync();
            }
        }
    }
}
