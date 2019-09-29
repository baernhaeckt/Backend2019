using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Persistence.Abstraction
{
    public interface IReader
    {
        Task<long> CountAsync<TEntity>()
            where TEntity : Entity, new();

        Task<IEnumerable<TEntity>> GetAllAsync<TEntity>()
            where TEntity : Entity, new();

        Task<TEntity> GetAsync<TEntity>(Guid id)
            where TEntity : Entity, new();

        Task<IEnumerable<TEntity>> WhereAsync<TEntity>(Expression<Func<TEntity, bool>> predicate)
            where TEntity : Entity, new();

        Task<TEntity> FirstOrDefaultAsync<TEntity>(Expression<Func<TEntity, bool>> predicate)
            where TEntity : Entity, new();

        Task<TEntity> SingleOrDefaultAsync<TEntity>(Expression<Func<TEntity, bool>> predicate)
            where TEntity : Entity, new();
    }
}