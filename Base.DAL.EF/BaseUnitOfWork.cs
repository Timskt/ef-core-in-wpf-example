using System.Threading.Tasks;
using Base.Contracts.DAL;
using Microsoft.EntityFrameworkCore;

namespace Base.DAL.EF
{
    public abstract class BaseUnitOfWork<TDbContext> : IUnitOfWork
        where TDbContext : DbContext
    {
        protected readonly TDbContext UowDbContext;

        protected BaseUnitOfWork(TDbContext dbContext)
        {
            UowDbContext = dbContext;
        }

        public virtual async Task<int> SaveChangesAsync()
        {
            return await UowDbContext.SaveChangesAsync();
        }

        public virtual int SaveChanges()
        {
            return UowDbContext.SaveChanges();
        }

        public void Dispose()
        {
            UowDbContext.Dispose();
        }
    }
}