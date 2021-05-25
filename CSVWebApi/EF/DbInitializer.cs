using System;
using System.Linq;
using CSVWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CSVWebApi.EF
{
    public static class DbInitializer
    {
        public static void Initialize(TestDbContext context)
        {
            if (context.Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
            {
                context.Database.Migrate();
            }

            if (!context.TestModels.Any())
            {
                CreateTestModels(context);
            }
        }

        private static DateTime GetRandomDay()
        {
            var rnd = new Random();
            var  start = new DateTime(2021, 1, 1);
            int range = (DateTime.Today - start).Days;
            return start.AddDays(rnd.Next(range));
        }

        private static void CreateTestModels(TestDbContext context)
        {
            using var transaction = context.Database.BeginTransaction();

            try
            {
                var rnd = new Random();

                for (int i = 0; i < 1000; i++)
                {
                    var rndDate = GetRandomDay();
                    var testModel = new TestModel
                    {
                        Name = "Test name",
                        Description = "Test description",
                        CreatedAt = rndDate,
                        IsDeleted = rnd.Next(0, 2) == 1,
                        EditedAt = rndDate
                    };

                    context.TestModels.Add(testModel);
                }

                context.SaveChanges();
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
            }
        }
    }
}
