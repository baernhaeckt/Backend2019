using System.Threading.Tasks;
using Backend.Core.Events;
using Backend.Core.Features.Newsfeed.Abstraction;
using Backend.Core.Features.Newsfeed.Events;
using Backend.Core.Framework;
using Microsoft.Extensions.Logging;

namespace Backend.Core.Features.Newsfeed.EventSubscribers
{
    internal class UserNewPointsEventHandler : EventSubscriber<UserNewPointsEvent>
    {
        private readonly IEventFeed _eventFeed;

        public UserNewPointsEventHandler(IEventFeed eventFeed, ILogger<UserNewPointsEventHandler> logger)
            : base(logger) => _eventFeed = eventFeed;

        public override async Task ExecuteAsync(UserNewPointsEvent @event)
        {
            Logger.HandleUserNewPointsEvent(@event.User.Id, @event.Points, @event.Co2Saving);

            // TODO: This should be non-blocking..
            await _eventFeed.PublishAsync(new PointsReceivedNewsfeedEvent(@event.User, @event.Points));
            await _eventFeed.PublishAsync(new FriendNewsfeedPointsReceivedEvent(@event.User, @event.Points));

            Logger.HandleUserNewPointsEventSuccessful(@event.User.Id, @event.Points, @event.Co2Saving);
        }
    }
}