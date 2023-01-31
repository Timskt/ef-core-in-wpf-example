using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace App.DAL.EF
{
    public class ExampleDbContextFactory : IDesignTimeDbContextFactory<ExampleDbContext>
    {
        public ExampleDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ExampleDbContext>();
            optionsBuilder.UseSqlite("Data Source=ExampleDatabase.db");

            return new ExampleDbContext(optionsBuilder.Options);
        }
    }
}