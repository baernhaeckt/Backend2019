using System.Threading.Tasks;
using Backend.Core.Features.Newsfeed.Abstraction;
using Backend.Core.Features.Newsfeed.Events;
using Silverback.Messaging.Subscribers;

namespace Backend.Core.Features.Newsfeed.EventHandler
{
    internal class UserNewPointsEventHandler : ISubscriber
    {
        private readonly IEventFeed _eventFeed;

        public UserNewPointsEventHandler(IEventFeed eventFeed)
        {
            _eventFeed = eventFeed;
        }

        public async Task ExecuteAsync(Core.Events.UserNewPointsEvent @event)
        {
            // TODO: This should be non-blocking..
            await _eventFeed.PublishAsync(new PointsReceivedNewsfeedEvent(@event.User, @event.Points));
            await _eventFeed.PublishAsync(new FriendNewsfeedPointsReceivedEvent(@event.User, @event.Points));
        }
    }
}