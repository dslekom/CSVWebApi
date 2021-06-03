using System;
using System.Threading.Tasks;
using Grpc.Core;
using WebApi.DAL.Entities;
using WebApi.DAL.Interfaces;

namespace WebApi.gRPC.Services
{
    public class TestModelService : TestModelImport.TestModelImportBase
    {
        private readonly IUnitOfWork database;

        public TestModelService(IUnitOfWork database)
        {
            this.database = database;
        }

        public override async Task<TestModelResponse> Add(TestModelRequest request, ServerCallContext context)
        {
            var now = DateTime.Now;

            var testModel = new TestModel
            {
                Name = request.Name,
                Description = request.Description,
                CreatedAt = now,
                EditedAt = now
            };

            await database.GetRepository<TestModel>().CreateAsync(testModel);
            await database.SaveAsync();

            return new TestModelResponse
            {
                Id = testModel.ID,
                Name = testModel.Name,
                Description = testModel.Description
            };
        }
    }
}
