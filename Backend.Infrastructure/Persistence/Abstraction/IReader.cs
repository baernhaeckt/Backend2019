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

        Task<long> CountAsync<TEntity>(Expression<Func<TEntity, bool>> filterPredicate)
            where TEntity : Entity, new();

        Task<IEnumerable<TEntity>> GetAllAsync<TEntity>()
            where TEntity : Entity, new();

        Task<TEntity> GetByIdOrDefaultAsync<TEntity>(Guid id)
            where TEntity : Entity, new();

        Task<TEntity> GetByIdOrThrowAsync<TEntity>(Guid id)
            where TEntity : Entity, new();

        Task<TProjection> GetByIdOrDefaultAsync<TEntity, TProjection>(Guid id, Expression<Func<TEntity, TProjection>> selectPredicate)
            where TEntity : Entity, new()
            where TProjection : class;

        Task<TProjection> GetByIdOrThrowAsync<TEntity, TProjection>(Guid id, Expression<Func<TEntity, TProjection>> selectPredicate)
            where TEntity : Entity, new()
            where TProjection : class;

        Task<IEnumerable<TEntity>> WhereAsync<TEntity>(Expression<Func<TEntity, bool>> filterPredicate)
            where TEntity : Entity, new();

        Task<TEntity> FirstOrDefaultAsync<TEntity>(Expression<Func<TEntity, bool>> filterPredicate)
            where TEntity : Entity, new();

        Task<TEntity> SingleOrDefaultAsync<TEntity>(Expression<Func<TEntity, bool>> filterPredicate)
            where TEntity : Entity, new();

        Task<TEntity> SingleAsync<TEntity>(Expression<Func<TEntity, bool>> filterPredicate)
            where TEntity : Entity, new();

        Task<TProjection> SingleAsync<TEntity, TProjection>(Expression<Func<TEntity, bool>> filterPredicate, Expression<Func<TEntity, TProjection>> selectPredicate)
            where TEntity : Entity, new()
            where TProjection : class;
    }
}