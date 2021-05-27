using System;
using System.Linq;
using WebApi.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace WebApi.DAL.EF
{
    public static class DbInitializer
    {
        /// <summary>
        /// Инициализация бд тестовыми данными
        /// </summary>
        /// <param name="context"></param>
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

        /// <summary>
        /// Получение рандомной даты в период от минимальной переданной до текущей
        /// </summary>
        /// <param name="minDate"></param>
        /// <returns></returns>
        private static DateTime GetRandomDay(DateTime? minDate)
        {
            var rnd = new Random();
            var start = minDate ?? DateTime.MinValue;
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
                    var rndDate = GetRandomDay(new DateTime(2021, 1, 1));
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
