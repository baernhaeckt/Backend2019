using System.Threading.Tasks;
using Backend.Infrastructure.Abstraction.Persistence;
using Microsoft.Extensions.Logging;
using Silverback.Messaging.Messages;
using Silverback.Messaging.Subscribers;

namespace Backend.Core.Framework
{
    public abstract class CommandHandlerWithReturnValue<TCommand, TResult> : ISubscriber
        where TCommand : ICommand
    {
        protected CommandHandlerWithReturnValue(IUnitOfWork unitOfWork, ILogger logger)
        {
            UnitOfWork = unitOfWork;
            Logger = logger;
        }

        protected IUnitOfWork UnitOfWork { get; }

        protected ILogger Logger { get; }

        public abstract Task<TResult> ExecuteAsync(TCommand command);
    }
}