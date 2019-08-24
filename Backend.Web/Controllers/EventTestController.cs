using Backend.Core.Newsfeed;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Web.Controllers
{
    [Route("api/events")]
    [ApiController]
    [AllowAnonymous]
    public class EventTestController : ControllerBase
    {
        private readonly IEventStream _eventStream;

        public EventTestController(IEventStream eventStream)
        {
            _eventStream = eventStream;
        }

        [HttpGet(nameof(PointsReceived))]
        public void PointsReceived()
        {
            _eventStream.PublishAsync(new PointsReceivedEvent());
        }

        [HttpGet(nameof(BadgeReceived))]
        public void BadgeReceived()
        {
            _eventStream.PublishAsync(new BadgeReceivedEvent());
        }

        [HttpGet(nameof(FriendBadgeReceived))]
        public void FriendBadgeReceived()
        {
            _eventStream.PublishAsync(new FriendBadgeReceivedEvent());
        }

        [HttpGet(nameof(FriendPointsReceived))]
        public void FriendPointsReceived()
        {
            _eventStream.PublishAsync(new FriendPointsReceivedEvent());
        }
    }
}
