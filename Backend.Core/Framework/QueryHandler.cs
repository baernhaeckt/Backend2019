using System.Threading.Tasks;
using Backend.Infrastructure.Abstraction.Persistence;
using Microsoft.Extensions.Logging;
using Silverback.Messaging.Messages;
using Silverback.Messaging.Subscribers;

namespace Backend.Core.Framework
{
    public abstract class QueryHandler<TResult, TQuery> : ISubscriber
        where TResult : class
        where TQuery : IQuery<TResult>
    {
        protected QueryHandler(IReader reader, ILogger logger)
        {
            Logger = logger;
            Reader = reader;
        }

        protected ILogger Logger { get; }

        protected IReader Reader { get; }

        public abstract Task<TResult> ExecuteAsync(TQuery query);
    }
}