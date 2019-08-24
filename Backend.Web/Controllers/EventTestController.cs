using Backend.Core.Hubs;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Web.Controllers
{
    [Route("api/events")]
    [ApiController]
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
            _eventStream.PublishAsync(new Event
            {
                title = "Punkte erhalten",
                message = "Gratulation, du hast neue Punkte erhalten!",
                variant = "info"
            });
        }

        [HttpGet(nameof(BadgeReceived))]
        public void BadgeReceived()
        {
            _eventStream.PublishAsync(new Event
            {
                title = "Punkte erhalten",
                message = "Gratulation, du hast einen neuen Award erhalten!",
                variant = "success"
            });
        }

        [HttpGet(nameof(FriendBadgeReceived))]
        public void FriendBadgeReceived()
        {
            _eventStream.PublishAsync(new Event
            {
                title = "Freund hat Award erhalten",
                message = "Hei! Einer deiner Freunde hat einen Award erhalten.",
                variant = "info"
            });
        }

        [HttpGet(nameof(FriendPointsReceived))]
        public void FriendPointsReceived()
        {
            _eventStream.PublishAsync(new Event
            {
                title = "Freund hat Punkte erhalten",
                message = "Hei! Einer deiner Freunde hat Punkte erhalten.",
                variant = "info"
            });
        }
    }
}
