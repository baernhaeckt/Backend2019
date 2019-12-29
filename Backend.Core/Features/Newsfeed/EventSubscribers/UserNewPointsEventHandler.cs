using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Core.Entities;
using Backend.Core.Events;
using Backend.Core.Features.Newsfeed.Abstraction;
using Backend.Core.Features.Newsfeed.Events;
using Backend.Core.Framework.Cqrs;
using Backend.Infrastructure.Abstraction.Persistence;
using Microsoft.Extensions.Logging;

namespace Backend.Core.Features.Newsfeed.EventSubscribers
{
    internal class UserNewPointsEventHandler : EventSubscriber<UserNewPointsEvent>
    {
        private readonly IEventFeed _eventFeed;

        private readonly IReader _reader;

        public UserNewPointsEventHandler(IEventFeed eventFeed, ILogger<UserNewPointsEventHandler> logger, IReader reader)
            : base(logger)
        {
            _eventFeed = eventFeed;
            _reader = reader;
        }

        public override async Task ExecuteAsync(UserNewPointsEvent @event)
        {
            Logger.HandleUserNewPointsEvent(@event.UserId, @event.Points, @event.Co2Saving);

            IEnumerable<Guid> friends = await _reader.GetByIdOrThrowAsync<User, IEnumerable<Guid>>(@event.UserId, user => user.Friends);

            await _eventFeed.PublishAsync(new PointsReceivedNewsfeedEvent(@event.UserId, @event.Points));
            await _eventFeed.PublishAsync(new FriendNewsfeedPointsReceivedEvent(@event.UserDisplayName, friends, @event.Points));

            Logger.HandleUserNewPointsEventSuccessful(@event.UserId, @event.Points, @event.Co2Saving);
        }
    }
}