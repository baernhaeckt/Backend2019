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
    internal class UserNewAwardEventHandler : EventSubscriber<UserNewAwardEvent>
    {
        private readonly IEventFeed _eventFeed;

        private readonly IReader _reader;

        public UserNewAwardEventHandler(IEventFeed eventFeed, ILogger<UserNewAwardEventHandler> logger, IReader reader)
            : base(logger)
        {
            _eventFeed = eventFeed;
            _reader = reader;
        }

        public override async Task ExecuteAsync(UserNewAwardEvent @event)
        {
            Logger.HandleNewAwardEvent(@event.UserId, @event.AwardName);

            IEnumerable<Guid> friends = await _reader.GetByIdOrThrowAsync<User, IEnumerable<Guid>>(@event.UserId, user => user.Friends);

            await _eventFeed.PublishAsync(new AwardReceivedNewsfeedEvent(@event.UserId, @event.AwardName));
            await _eventFeed.PublishAsync(new FriendNewsfeedAwardReceivedEvent(@event.UserDisplayName, friends, @event.AwardName));

            Logger.HandleNewAwardEventSuccessful(@event.UserId, @event.AwardName);
        }
    }
}