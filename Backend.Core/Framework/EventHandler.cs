using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Silverback.Messaging.Messages;
using Silverback.Messaging.Subscribers;

namespace Backend.Core.Framework
{
    public abstract class EventHandler<TEvent> : ISubscriber
        where TEvent : IEvent
    {
        protected EventHandler(ILogger logger) => Logger = logger;

        protected ILogger Logger { get; }

        public abstract Task ExecuteAsync(TEvent @event);
    }
}