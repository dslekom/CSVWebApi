using CSVWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CSVWebApi.EF
{
    public class TestDbContext : DbContext
    {
        public TestDbContext(DbContextOptions<TestDbContext> options)
            : base(options)
        { }

        public DbSet<TestModel> TestModels { get; set; }
    }
}
