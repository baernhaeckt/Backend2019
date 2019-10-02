using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
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

        public virtual async Task UpdateAsync<TEntity>(Guid id, object definition)
            where TEntity : Entity, new()
        {
            DbContext dbContext = DbContextFactory.Create();

            IList<UpdateDefinition<TEntity>> updateDefinitions = new List<UpdateDefinition<TEntity>>();
            foreach (PropertyInfo property in definition.GetType().GetProperties())
            {
                UpdateDefinitionBuilder<TEntity> builder = Builders<TEntity>.Update;
                if (property.PropertyType.GetInterfaces()
                    .Any(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(ICollection<>)))
                {
                    // This will not replace child collections but add the elements to it.
                    updateDefinitions.Add(builder.PushEach(property.Name, (IEnumerable<object>)property.GetValue(definition)));
                }
                else
                {
                    updateDefinitions.Add(builder.Set(property.Name, property.GetValue(definition)));
                }
            }

            UpdateDefinition<TEntity> updateDefinition = Builders<TEntity>.Update.Combine(updateDefinitions);
            FilterDefinition<TEntity> filter = Builders<TEntity>.Filter.Eq(nameof(Entity.Id), id);
            await dbContext.GetCollection<TEntity>().UpdateOneAsync(filter, updateDefinition);
        }
    }
}