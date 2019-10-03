using System.Threading.Tasks;
using Backend.Core.Entities;
using Backend.Core.Entities.Awards;
using Backend.Core.Extensions;
using Backend.Core.Features.Newsfeed.Abstraction;
using Backend.Core.Features.Newsfeed.Events;
using Backend.Infrastructure.Persistence.Abstraction;
using Backend.Infrastructure.Security.Abstraction;
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
        public async Task PointsReceived()
        {
            User user = await _reader.GetByIdOrThrowAsync<User>(HttpContext.User.Id());
            await _eventFeed.PublishAsync(new PointsReceivedNewsfeedEvent(user, 5));
        }

        [HttpGet(nameof(BadgeReceived))]
        public async Task BadgeReceived()
        {
            User user = await _reader.GetByIdOrThrowAsync<User>(HttpContext.User.Id());
            await _eventFeed.PublishAsync(new AwardReceivedNewsfeedEvent(user, new TrashHeroAward()));
        }

        [HttpGet(nameof(FriendBadgeReceived))]
        public async Task FriendBadgeReceived()
        {
            User user = await _reader.GetByIdOrThrowAsync<User>(HttpContext.User.Id());
            await _eventFeed.PublishAsync(new FriendNewsfeedAwardReceivedEvent(user, new TrashHeroAward()));
        }

        [HttpGet(nameof(FriendPointsReceived))]
        public async Task FriendPointsReceived()
        {
            User user = await _reader.GetByIdOrThrowAsync<User>(HttpContext.User.Id());
            await _eventFeed.PublishAsync(new FriendNewsfeedPointsReceivedEvent(user, 5));
        }
    }
}