using System.Threading.Tasks;
using Backend.Core.Features.Newsfeed.Events;
using Backend.Core.Features.UserManagement;
using Backend.Core.Features.UserManagement.Security;
using Backend.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Core.Features.Newsfeed.Controllers
{
    [Route("api/events")]
    [ApiController]
    [Authorize(Roles = Roles.Administrator)]
    public class EventTestController : ControllerBase
    {
        private readonly IEventStream _eventStream;
        private readonly UserService _userService;

        public EventTestController(IEventStream eventStream, UserService userService)
        {
            _eventStream = eventStream;
            _userService = userService;
        }

        [HttpGet(nameof(PointsReceivedAsync))]
        public async Task PointsReceivedAsync()
        {
            await _eventStream.PublishAsync(new PointsReceivedEvent(await _userService.GetCurrentUser(), 5));
        }

        [HttpGet(nameof(BadgeReceivedAsync))]
        public async Task BadgeReceivedAsync()
        {
            await _eventStream.PublishAsync(new BadgeReceivedEvent(await _userService.GetCurrentUser(), new TrashHeroAward()));
        }

        [HttpGet(nameof(FriendBadgeReceivedAsync))]
        public async Task FriendBadgeReceivedAsync()
        {
            await _eventStream.PublishAsync(new FriendBadgeReceivedEvent(await _userService.GetCurrentUser(), new TrashHeroAward()));
        }

        [HttpGet(nameof(FriendPointsReceivedAsync))]
        public async Task FriendPointsReceivedAsync()
        {
            await _eventStream.PublishAsync(new FriendPointsReceivedEvent(await _userService.GetCurrentUser(), 5));
        }
    }
}
