using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Backend.Infrastructure.Persistence.Abstraction;
using MongoDB.Driver;

namespace Backend.Infrastructure.Persistence
{
    internal class MongoDbReader : IReader
    {
        public MongoDbReader(DbContextFactory dbContextFactory) => DbContextFactory = dbContextFactory;

        protected DbContextFactory DbContextFactory { get; }

        public virtual async Task<TEntity> GetByIdOrDefaultAsync<TEntity>(Guid id)
            where TEntity : Entity, new() =>
            await SingleOrDefaultAsync<TEntity>(e => e.Id == id);

        public async Task<TEntity> GetByIdOrThrowAsync<TEntity>(Guid id)
            where TEntity : Entity, new()
        {
            TEntity result = await GetByIdOrDefaultAsync<TEntity>(id);
            return result ?? throw new EntityNotFoundException(typeof(TEntity), nameof(GetByIdOrThrowAsync), id.ToString());
        }

        public Task<TProjection?> GetByIdOrDefaultAsync<TEntity, TProjection>(Guid id, Expression<Func<TEntity, TProjection>> selectPredicate)
            where TEntity : Entity, new()
            where TProjection : class =>
            SingleOrDefaultAsync(e => e.Id == id, selectPredicate);

        public async Task<TProjection> GetByIdOrThrowAsync<TEntity, TProjection>(Guid id, Expression<Func<TEntity, TProjection>> selectPredicate)
            where TEntity : Entity, new()
            where TProjection : class
        {
            TProjection? result = await GetByIdOrDefaultAsync(id, selectPredicate);
            return result ?? throw new EntityNotFoundException(typeof(TEntity), nameof(GetByIdOrThrowAsync), id.ToString());
        }

        public async Task<long> CountAsync<TEntity>(Expression<Func<TEntity, bool>> filterPredicate)
            where TEntity : Entity, new()
        {
            DbContext dbContext = DbContextFactory.Create();
            FilterDefinition<TEntity> filter = new ExpressionFilterDefinition<TEntity>(filterPredicate);
            return await dbContext.GetCollection<TEntity>().CountDocumentsAsync(filter);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync<TEntity>()
            where TEntity : Entity, new()
        {
            DbContext dbContext = DbContextFactory.Create();
            return (await dbContext.GetCollection<TEntity>().FindAsync(FilterDefinition<TEntity>.Empty)).ToEnumerable();
        }

        public async Task<long> CountAsync<TEntity>()
            where TEntity : Entity, new()
        {
            DbContext dbContext = DbContextFactory.Create();
            return await dbContext.GetCollection<TEntity>().CountDocumentsAsync(FilterDefinition<TEntity>.Empty);
        }

        public async Task<IEnumerable<TEntity>> WhereAsync<TEntity>(Expression<Func<TEntity, bool>> filterPredicate)
            where TEntity : Entity, new()
        {
            DbContext dbContext = DbContextFactory.Create();
            FilterDefinition<TEntity> filter = new ExpressionFilterDefinition<TEntity>(filterPredicate);
            return (await dbContext.GetCollection<TEntity>().FindAsync(filter)).ToEnumerable();
        }

        public async Task<TEntity> FirstOrDefaultAsync<TEntity>(Expression<Func<TEntity, bool>> filterPredicate)
            where TEntity : Entity, new()
        {
            DbContext dbContext = DbContextFactory.Create();
            FilterDefinition<TEntity> filter = new ExpressionFilterDefinition<TEntity>(filterPredicate);
            return await (await dbContext.GetCollection<TEntity>().FindAsync(filter)).FirstOrDefaultAsync();
        }

        public async Task<TEntity> SingleOrDefaultAsync<TEntity>(Expression<Func<TEntity, bool>> filterPredicate)
            where TEntity : Entity, new()
        {
            DbContext dbContext = DbContextFactory.Create();
            FilterDefinition<TEntity> filter = new ExpressionFilterDefinition<TEntity>(filterPredicate);
            return await (await dbContext.GetCollection<TEntity>().FindAsync(filter)).FirstOrDefaultAsync();
        }

        public async Task<TEntity> SingleAsync<TEntity>(Expression<Func<TEntity, bool>> filterPredicate)
            where TEntity : Entity, new()
        {
            TEntity result = await SingleOrDefaultAsync(filterPredicate);
            return result ?? throw new EntityNotFoundException(typeof(TEntity), nameof(SingleAsync), filterPredicate.ToString());
        }

        public async Task<TProjection> SingleAsync<TEntity, TProjection>(Expression<Func<TEntity, bool>> filterPredicate, Expression<Func<TEntity, TProjection>> selectPredicate)
            where TEntity : Entity, new()
            where TProjection : class
        {
            TProjection? result = await SingleOrDefaultAsync(filterPredicate, selectPredicate);
            return result ?? throw new EntityNotFoundException(typeof(TEntity), nameof(SingleAsync), filterPredicate.ToString());
        }

        public async Task<TProjection?> SingleOrDefaultAsync<TEntity, TProjection>(Expression<Func<TEntity, bool>> filterPredicate, Expression<Func<TEntity, TProjection>> selectPredicate)
            where TEntity : Entity, new()
            where TProjection : class
        {
            DbContext dbContext = DbContextFactory.Create();
            FilterDefinition<TEntity> filter = new ExpressionFilterDefinition<TEntity>(filterPredicate);
            List<TProjection> resultList = await dbContext.GetCollection<TEntity>().Find(filter).Project(selectPredicate).ToListAsync();
            return resultList.SingleOrDefault();
        }
    }
}