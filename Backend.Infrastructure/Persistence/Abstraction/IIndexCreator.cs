using System.Threading;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Persistence.Abstraction
{
    public interface IIndexCreator
    {
        Task Create<TEntity>(string field, CancellationToken cancellationToken)
            where TEntity : Entity, new();
    }
}