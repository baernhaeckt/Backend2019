using System.Threading;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Abstraction.Persistence
{
    public interface IIndexCreator
    {
        Task Create<TEntity>(string field, CancellationToken cancellationToken)
            where TEntity : IEntity, new();
    }
}