using System;
using System.Threading.Tasks;

namespace Base.Contracts.DAL
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> SaveChangesAsync();
        int SaveChanges();
    }
}