using System.Threading.Tasks;
using Backend.Core.Entities.Awards;
using Backend.Core.Features.Newsfeed.Abstraction;
using Backend.Core.Features.Newsfeed.Events;
using Backend.Core.Features.UserManagement;
using Backend.Core.Features.UserManagement.Security;
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

        private readonly UserService _userService;

        public EventTestController(IEventFeed eventFeed, UserService userService)
        {
            _eventFeed = eventFeed;
            _userService = userService;
        }

        [HttpGet(nameof(PointsReceived))]
        public async Task PointsReceived()
        {
            await _eventFeed.PublishAsync(new PointsReceivedNewsfeedEvent(await _userService.GetCurrentUser(), 5));
        }

        [HttpGet(nameof(BadgeReceived))]
        public async Task BadgeReceived()
        {
            await _eventFeed.PublishAsync(new BadgeReceivedNewsfeedEvent(await _userService.GetCurrentUser(), new TrashHeroAward()));
        }

        [HttpGet(nameof(FriendBadgeReceived))]
        public async Task FriendBadgeReceived()
        {
            await _eventFeed.PublishAsync(new FriendNewsfeedBadgeReceivedEvent(await _userService.GetCurrentUser(), new TrashHeroAward()));
        }

        [HttpGet(nameof(FriendPointsReceived))]
        public async Task FriendPointsReceived()
        {
            await _eventFeed.PublishAsync(new FriendNewsfeedPointsReceivedEvent(await _userService.GetCurrentUser(), 5));
        }
    }
}