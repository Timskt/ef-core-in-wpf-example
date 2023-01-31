using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Base.Contracts.DAL;
using Base.Contracts.Domain;
using Base.Contracts.Mapper;
using Microsoft.EntityFrameworkCore;

namespace Base.DAL.EF
{
    public abstract class
        BaseEntityRepository<TDalEntity, TDomainEntity, TDbContext> : BaseEntityRepository<TDalEntity, TDomainEntity,
            TDbContext,
            int>
        where TDalEntity : class, IDomainEntityId<int>
        where TDomainEntity : class, IDomainEntityId<int>
        where TDbContext : DbContext
    {
        protected BaseEntityRepository(
            TDbContext dbContext,
            IMapper<TDalEntity, TDomainEntity> mapper
        ) : base(dbContext, mapper)
        {
        }
    }

    public abstract class
        BaseEntityRepository<TDalEntity, TDomainEntity, TDbContext, TKey> : IEntityRepository<TDalEntity, TKey>
        where TDalEntity : class, IDomainEntityId<TKey>
        where TDomainEntity : class, IDomainEntityId<TKey>
        where TDbContext : DbContext
        where TKey : IEquatable<TKey>
    {
        protected readonly IMapper<TDalEntity, TDomainEntity> Mapper;
        protected readonly TDbContext RepoDbContext;
        protected readonly DbSet<TDomainEntity> RepoDbSet;

        protected BaseEntityRepository(
            TDbContext dbContext,
            IMapper<TDalEntity, TDomainEntity> mapper
        )
        {
            RepoDbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            RepoDbSet = dbContext.Set<TDomainEntity>();
            Mapper = mapper;
        }

        public virtual IEnumerable<TDalEntity> All(bool noTracking = true)
        {
            var query = CreateQuery(noTracking);
            var entities = query.ToList();
            return entities.Select(e => Mapper.Map(e));
        }

        public virtual async Task<IEnumerable<TDalEntity>> AllAsync(bool noTracking = true)
        {
            var query = CreateQuery(noTracking);
            var entities = await query.ToListAsync();
            return entities.Select(e => Mapper.Map(e));
        }

        public virtual IEnumerable<TDalEntity> All(TKey id, bool noTracking = true)
        {
            var query = CreateQuery(noTracking);
            if (id != null) query = query.Where(e => e.Id.Equals(id));

            return query.ToList().Select(e => Mapper.Map(e));
        }

        public virtual async Task<IEnumerable<TDalEntity>> AllAsync(TKey id, bool noTracking = true)
        {
            var query = CreateQuery(noTracking);
            if (id != null) query = query.Where(e => e.Id.Equals(id));

            return (await query.ToListAsync()).Select(e => Mapper.Map(e));
        }

        public virtual TDalEntity FirstOrDefault(TKey id, bool noTracking = true)
        {
            var query = CreateQuery(noTracking);
            var domainEntity = query.FirstOrDefault(e => e.Id.Equals(id));
            var result = Mapper.Map(domainEntity);
            return result;
        }

        public virtual async Task<TDalEntity> FirstOrDefaultAsync(TKey id, bool noTracking = true)
        {
            var query = CreateQuery(noTracking);
            var domainEntity = await query.FirstOrDefaultAsync(e => e.Id.Equals(id));
            var result = Mapper.Map(domainEntity);
            return result;
        }

        public virtual TDalEntity Add(TDalEntity entity)
        {
            var domainEntity = Mapper.Map(entity);
            var result = RepoDbSet.Add(domainEntity).Entity;
            var dalEntity = Mapper.Map(result);
            return dalEntity;
        }

        public virtual async Task<TDalEntity> AddAsync(TDalEntity entity)
        {
            var domainEntity = Mapper.Map(entity);
            var result = (await RepoDbSet.AddAsync(domainEntity)).Entity;
            var dalEntity = Mapper.Map(result);
            return dalEntity;
        }

        public virtual TDalEntity Update(TDalEntity entity)
        {
            var domainEntity = Mapper.Map(entity);
            var result = RepoDbSet.Update(domainEntity).Entity;
            var dalEntity = Mapper.Map(result);
            return dalEntity;
        }

        public virtual TDalEntity Remove(TDalEntity entity)
        {
            var domainEntity = Mapper.Map(entity);
            var result = RepoDbSet.Remove(domainEntity).Entity;
            var dalEntity = Mapper.Map(result);
            return dalEntity;
        }

        public virtual TDalEntity Remove(TKey id)
        {
            var entity = FirstOrDefault(id);
            if (entity == null)
                // TODO: implement custom exception for entity not found
                throw new NullReferenceException($"Entity {typeof(TDalEntity).Name} with id {id} was not found");

            return Remove(entity);
        }

        public virtual async Task<TDalEntity> RemoveAsync(TKey id)
        {
            var entity = await FirstOrDefaultAsync(id);
            if (entity == null)
                // TODO: implement custom exception for entity not found
                throw new NullReferenceException($"Entity {typeof(TDalEntity).Name} with id {id} was not found");

            return Remove(entity);
        }

        public virtual bool Exists(TKey id)
        {
            return RepoDbSet.Any(a => a.Id.Equals(id));
        }

        public virtual async Task<bool> ExistsAsync(TKey id)
        {
            return await RepoDbSet.AnyAsync(a => a.Id.Equals(id));
        }

        protected virtual IQueryable<TDomainEntity> CreateQuery(bool noTracking = true)
        {
            var query = RepoDbSet.AsQueryable();
            if (noTracking) query = query.AsNoTracking();

            return query;
        }
    }
}