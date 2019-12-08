using System;
using System.Threading.Tasks;
using Backend.Core.Entities;
using Backend.Core.Entities.Awards;
using Backend.Core.Features.Newsfeed.Abstraction;
using Backend.Core.Features.Newsfeed.Events;
using Backend.Infrastructure.Abstraction.Persistence;
using Backend.Infrastructure.Abstraction.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Core.Features.Newsfeed.Controllers
{
    [Route("api/events")]
    [ApiController]
    [Authorize(Roles = Roles.Administrator)]
    public class EventTestController : ControllerBase
    {
        private readonly IEventFeed _eventFeed;

        private readonly IReader _reader;

        public EventTestController(IEventFeed eventFeed, IReader reader)
        {
            _eventFeed = eventFeed;
            _reader = reader;
        }

        [HttpGet(nameof(PointsReceived))]
        public async Task PointsReceived(Guid userId)
        {
            User user = await _reader.GetByIdOrThrowAsync<User>(userId);
            await _eventFeed.PublishAsync(new PointsReceivedNewsfeedEvent(user.Id, 5));
        }

        [HttpGet(nameof(BadgeReceived))]
        public async Task BadgeReceived(Guid userId)
        {
            User user = await _reader.GetByIdOrThrowAsync<User>(userId);
            await _eventFeed.PublishAsync(new AwardReceivedNewsfeedEvent(user.Id, new TrashHeroAward().Title));
        }

        [HttpGet(nameof(FriendBadgeReceived))]
        public async Task FriendBadgeReceived(Guid userId)
        {
            User user = await _reader.GetByIdOrThrowAsync<User>(userId);
            await _eventFeed.PublishAsync(new FriendNewsfeedAwardReceivedEvent(user.DisplayName, user.Friends, new TrashHeroAward().Title));
        }

        [HttpGet(nameof(FriendPointsReceived))]
        public async Task FriendPointsReceived(Guid userId)
        {
            User user = await _reader.GetByIdOrThrowAsync<User>(userId);
            await _eventFeed.PublishAsync(new FriendNewsfeedPointsReceivedEvent(user.DisplayName, user.Friends, 5));
        }
    }
}