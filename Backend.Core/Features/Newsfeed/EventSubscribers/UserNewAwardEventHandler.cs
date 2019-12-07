using System.Threading.Tasks;
using Backend.Core.Events;
using Backend.Core.Features.Newsfeed.Abstraction;
using Backend.Core.Features.Newsfeed.Events;
using Backend.Core.Framework;
using Microsoft.Extensions.Logging;

namespace Backend.Core.Features.Newsfeed.EventSubscribers
{
    internal class UserNewAwardEventHandler : EventSubscriber<UserNewAwardEvent>
    {
        private readonly IEventFeed _eventFeed;

        public UserNewAwardEventHandler(IEventFeed eventFeed, ILogger<UserNewAwardEventHandler> logger)
            : base(logger) => _eventFeed = eventFeed;

        public override async Task ExecuteAsync(UserNewAwardEvent @event)
        {
            Logger.HandleNewAwardEvent(@event.User.Id, @event.Award.Kind);

            // TODO: This should be non-blocking..
            await _eventFeed.PublishAsync(new AwardReceivedNewsfeedEvent(@event.User, @event.Award));
            await _eventFeed.PublishAsync(new FriendNewsfeedAwardReceivedEvent(@event.User, @event.Award));

            Logger.HandleNewAwardEventSuccessful(@event.User.Id, @event.Award.Kind);
        }
    }
}