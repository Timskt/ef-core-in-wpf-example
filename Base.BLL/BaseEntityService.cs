using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Base.Contracts.BLL;
using Base.Contracts.DAL;
using Base.Contracts.Domain;
using Base.Contracts.Mapper;

namespace Base.BLL
{
    public abstract class
        BaseEntityService<TBllEntity, TDalEntity, TRepository> :
            BaseEntityService<TBllEntity, TDalEntity, TRepository, int>, IEntityService<TBllEntity>
        where TDalEntity : class, IDomainEntityId
        where TBllEntity : class, IDomainEntityId
        where TRepository : IEntityRepository<TDalEntity>
    {
        protected BaseEntityService(TRepository repository, IMapper<TBllEntity, TDalEntity> mapper) : base(repository,
            mapper)
        {
        }
    }

    public abstract class
        BaseEntityService<TBllEntity, TDalEntity, TRepository, TKey> : IEntityService<TBllEntity, TKey>
        where TBllEntity : class, IDomainEntityId<TKey>
        where TDalEntity : class, IDomainEntityId<TKey>
        where TRepository : IEntityRepository<TDalEntity, TKey>
        where TKey : IEquatable<TKey>
    {
        protected IMapper<TBllEntity, TDalEntity> Mapper;
        protected TRepository Repository;

        protected BaseEntityService(TRepository repository, IMapper<TBllEntity, TDalEntity> mapper)
        {
            Repository = repository;
            Mapper = mapper;
        }


        public virtual IEnumerable<TBllEntity> All(bool noTracking = true)
        {
            return Repository.All(noTracking).Select(e => Mapper.Map(e));
        }

        public virtual async Task<IEnumerable<TBllEntity>> AllAsync(bool noTracking = true)
        {
            return (await Repository.AllAsync(noTracking)).Select(e => Mapper.Map(e));
        }

        public virtual IEnumerable<TBllEntity> All(TKey id, bool noTracking = true)
        {
            return Repository.All(id, noTracking).Select(e => Mapper.Map(e));
        }

        public virtual async Task<IEnumerable<TBllEntity>> AllAsync(TKey id, bool noTracking = true)
        {
            return (await Repository.AllAsync(id, noTracking)).Select(e => Mapper.Map(e));
        }

        public virtual TBllEntity FirstOrDefault(TKey id, bool noTracking = true)
        {
            return Mapper.Map(Repository.FirstOrDefault(id, noTracking));
        }

        public virtual async Task<TBllEntity> FirstOrDefaultAsync(TKey id, bool noTracking = true)
        {
            return Mapper.Map(await Repository.FirstOrDefaultAsync(id, noTracking));
        }

        public virtual TBllEntity Add(TBllEntity entity)
        {
            return Mapper.Map(Repository.Add(Mapper.Map(entity)));
        }

        public virtual async Task<TBllEntity> AddAsync(TBllEntity entity)
        {
            return Mapper.Map(await Repository.AddAsync(Mapper.Map(entity)));
        }

        public virtual TBllEntity Update(TBllEntity entity)
        {
            return Mapper.Map(Repository.Update(Mapper.Map(entity)));
        }

        public virtual TBllEntity Remove(TBllEntity entity)
        {
            return Mapper.Map(Repository.Remove(Mapper.Map(entity)));
        }

        public virtual TBllEntity Remove(TKey id)
        {
            return Mapper.Map(Repository.Remove(id));
        }

        public virtual async Task<TBllEntity> RemoveAsync(TKey id)
        {
            return Mapper.Map(await Repository.RemoveAsync(id));
        }

        public virtual bool Exists(TKey id)
        {
            return Repository.Exists(id);
        }

        public virtual Task<bool> ExistsAsync(TKey id)
        {
            return Repository.ExistsAsync(id);
        }
    }
}