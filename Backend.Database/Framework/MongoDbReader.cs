using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Backend.Database.Abstraction;
using Backend.Database.Entities;
using MongoDB.Driver;

namespace Backend.Database.Framework
{
    public class MongoDbReader : IReader
    {
        public MongoDbReader(DbContextFactory dbContextFactory)
        {
            DbContextFactory = dbContextFactory;
        }

        protected DbContextFactory DbContextFactory { get; }

        public virtual async Task<TEntity> GetAsync<TEntity>(Guid id)
            where TEntity : Entity, new()
        {
            DbContext dbContext = DbContextFactory.Create();
            FilterDefinition<TEntity> filter = new ExpressionFilterDefinition<TEntity>(u => u.Id == id);
            TEntity entity = await dbContext.GetCollection<TEntity>().Find(filter).SingleOrDefaultAsync();
            return entity;
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
            return await dbContext.GetCollection<TEntity>().EstimatedDocumentCountAsync();
        }

        public async Task<IEnumerable<TEntity>> WhereAsync<TEntity>(Expression<Func<TEntity, bool>> predicate)
            where TEntity : Entity, new()
        {
            DbContext dbContext = DbContextFactory.Create();
            FilterDefinition<TEntity> filter = new ExpressionFilterDefinition<TEntity>(predicate);
            return (await dbContext.GetCollection<TEntity>().FindAsync(filter)).ToEnumerable();
        }

        public async Task<TEntity> FirstOrDefaultAsync<TEntity>(Expression<Func<TEntity, bool>> predicate)
            where TEntity : Entity, new()
        {
            DbContext dbContext = DbContextFactory.Create();
            FilterDefinition<TEntity> filter = new ExpressionFilterDefinition<TEntity>(predicate);
            return await (await dbContext.GetCollection<TEntity>().FindAsync(filter)).FirstOrDefaultAsync();
        }

        public async Task<TEntity> SingleOrDefaultAsync<TEntity>(Expression<Func<TEntity, bool>> predicate)
            where TEntity : Entity, new()
        {
            DbContext dbContext = DbContextFactory.Create();
            FilterDefinition<TEntity> filter = new ExpressionFilterDefinition<TEntity>(predicate);
            return await (await dbContext.GetCollection<TEntity>().FindAsync(filter)).FirstOrDefaultAsync();
        }
    }
}