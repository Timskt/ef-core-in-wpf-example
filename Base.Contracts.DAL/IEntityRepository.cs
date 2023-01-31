using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Base.Contracts.Domain;

namespace Base.Contracts.DAL
{
    public interface IEntityRepository<TEntity> : IEntityRepository<TEntity, int>
        where TEntity : class, IDomainEntityId
    {
    }

    public interface IEntityRepository<TEntity, in TKey>
        where TEntity : class, IDomainEntityId<TKey>
        where TKey : IEquatable<TKey>
    {
        IEnumerable<TEntity> All(bool noTracking = true);
        Task<IEnumerable<TEntity>> AllAsync(bool noTracking = true);
        IEnumerable<TEntity> All(TKey id, bool noTracking = true);
        Task<IEnumerable<TEntity>> AllAsync(TKey id, bool noTracking = true);
        TEntity FirstOrDefault(TKey id, bool noTracking = true);
        Task<TEntity> FirstOrDefaultAsync(TKey id, bool noTracking = true);
        TEntity Add(TEntity entity);
        Task<TEntity> AddAsync(TEntity entity);
        TEntity Update(TEntity entity);
        TEntity Remove(TEntity entity);
        TEntity Remove(TKey id);
        Task<TEntity> RemoveAsync(TKey id);
        bool Exists(TKey id);
        Task<bool> ExistsAsync(TKey id);
    }
}