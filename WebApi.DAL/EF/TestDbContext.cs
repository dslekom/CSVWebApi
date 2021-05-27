using WebApi.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace WebApi.DAL.EF
{
    public class TestDbContext : DbContext
    {
        public TestDbContext(DbContextOptions<TestDbContext> options)
            : base(options)
        { }

        public DbSet<TestModel> TestModels { get; set; }
    }
}
