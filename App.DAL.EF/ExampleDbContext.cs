using App.Domain;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF
{
    public class ExampleDbContext : DbContext
    {
        public ExampleDbContext(DbContextOptions<ExampleDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Address> Addresses { get; set; }
    }
}