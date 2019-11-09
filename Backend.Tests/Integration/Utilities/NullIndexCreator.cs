using System.Threading;
using System.Threading.Tasks;
using Backend.Infrastructure.Abstraction.Persistence;

namespace Backend.Tests.Integration.Utilities
{
    public class NullIndexCreator : IIndexCreator
    {
        public Task Create<TEntity>(string field, CancellationToken cancellationToken)
            where TEntity : IEntity, new() => Task.CompletedTask;
    }
}