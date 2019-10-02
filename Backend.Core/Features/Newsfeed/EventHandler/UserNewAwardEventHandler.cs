using System.Threading.Tasks;
using Backend.Core.Events;
using Backend.Core.Features.Newsfeed.Abstraction;
using Backend.Core.Features.Newsfeed.Events;
using Silverback.Messaging.Subscribers;

namespace Backend.Core.Features.Newsfeed.EventHandler
{
    internal class UserNewAwardEventHandler : ISubscriber
    {
        private readonly IEventFeed _eventFeed;

        public UserNewAwardEventHandler(IEventFeed eventFeed) => _eventFeed = eventFeed;

        public async Task ExecuteAsync(UserNewAwardEvent @event)
        {
            // TODO: This should be non-blocking..
            await _eventFeed.PublishAsync(new AwardReceivedNewsfeedEvent(@event.User, @event.Award));
            await _eventFeed.PublishAsync(new FriendNewsfeedAwardReceivedEvent(@event.User, @event.Award));
        }
    }
}