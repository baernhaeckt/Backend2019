using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Abstraction.Persistence
{
    public interface IUnitOfWork : IReader
    {
        Task DeleteAsync<TEntity>(Guid id)
            where TEntity : IEntity, new();

        Task<TEntity> InsertAsync<TEntity>(TEntity record)
            where TEntity : IEntity, new();

        Task InsertManyAsync<TEntity>(IEnumerable<TEntity> records)
            where TEntity : IEntity, new();

        Task UpdateAsync<TEntity>(Guid id, object updateDefinition)
            where TEntity : IEntity, new();

        Task UpdateAsync<TEntity>(TEntity record)
            where TEntity : IEntity, new();
    }
}