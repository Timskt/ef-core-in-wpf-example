using System;
using System.Threading.Tasks;
using Base.Contracts.BLL;
using Base.Contracts.DAL;

namespace Base.BLL
{
    public abstract class BaseBLL<TUnitOfWork> : IBLL
        where TUnitOfWork : IUnitOfWork
    {
        public readonly TUnitOfWork UnitOfWork;

        protected BaseBLL(TUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        public virtual async Task<int> SaveChangesAsync()
        {
            return await UnitOfWork.SaveChangesAsync();
        }

        public virtual int SaveChanges()
        {
            return UnitOfWork.SaveChanges();
        }

        public void Dispose()
        {
            Console.WriteLine("BLL is disposed");
            UnitOfWork.Dispose();
        }
    }
}