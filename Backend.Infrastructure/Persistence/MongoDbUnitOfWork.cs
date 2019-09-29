using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Backend.Infrastructure.Persistence.Abstraction;
using MongoDB.Driver;

namespace Backend.Infrastructure.Persistence
{
    internal class MongoDbUnitOfWork : MongoDbReader, IUnitOfWork
    {
        public MongoDbUnitOfWork(DbContextFactory dbContextFactory)
            : base(dbContextFactory)
        {
        }

        public virtual async Task<TEntity> InsertAsync<TEntity>(TEntity record)
            where TEntity : Entity, new()
        {
            DbContext dbContext = DbContextFactory.Create();
            record.CreatedAt = DateTime.UtcNow;
            await dbContext.GetCollection<TEntity>().InsertOneAsync(record);
            return record;
        }

        public virtual async Task InsertManyAsync<TEntity>(IEnumerable<TEntity> records)
            where TEntity : Entity, new()
        {
            DbContext dbContext = DbContextFactory.Create();
            foreach (TEntity record in records)
            {
                record.CreatedAt = DateTime.UtcNow;
            }

            await dbContext.GetCollection<TEntity>().InsertManyAsync(records);
        }

        public virtual async Task DeleteAsync<TEntity>(Guid id)
            where TEntity : Entity, new()
        {
            var filter = new ExpressionFilterDefinition<TEntity>(collection => collection.Id == id);
            DeleteResult result = await DbContextFactory.Create().GetCollection<TEntity>().DeleteOneAsync(filter);
            if (result.DeletedCount < 0)
            {
                throw new ValidationException($"No record with id {id} found.");
            }
        }

        public virtual async Task UpdateAsync<TEntity>(TEntity record)
            where TEntity : Entity, new()
        {
            DbContext dbContext = DbContextFactory.Create();
            record.UpdatedAt = DateTime.UtcNow;
            FilterDefinition<TEntity> filter = new ExpressionFilterDefinition<TEntity>(u => u.Id == record.Id);
            await dbContext.GetCollection<TEntity>().ReplaceOneAsync(filter, record);
        }
    }
}