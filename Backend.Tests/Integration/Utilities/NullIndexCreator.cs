using System.Threading;
using System.Threading.Tasks;
using Backend.Infrastructure.Persistence.Abstraction;

namespace Backend.Tests.Integration.Utilities
{
    public class NullIndexCreator : IIndexCreator
    {
        public Task Create<TEntity>(string field, CancellationToken cancellationToken)
            where TEntity : Entity, new() => Task.CompletedTask;
    }
}