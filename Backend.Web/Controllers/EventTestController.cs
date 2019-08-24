﻿using Backend.Core.Newsfeed;
using Backend.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Web.Controllers
{
    [Route("api/events")]
    [ApiController]
    public class EventTestController : ControllerBase
    {
        private readonly IEventStream _eventStream;
        private readonly UserService _userService;

        public EventTestController(IEventStream eventStream, UserService userService)
        {
            _eventStream = eventStream;
            _userService = userService;
        }

        [HttpGet(nameof(PointsReceived))]
        public void PointsReceived()
        {
            _eventStream.PublishAsync(new PointsReceivedEvent(_userService.CurrentUser));
        }

        [HttpGet(nameof(BadgeReceived))]
        public void BadgeReceived()
        {
            _eventStream.PublishAsync(new BadgeReceivedEvent(_userService.CurrentUser));
        }

        [HttpGet(nameof(FriendBadgeReceived))]
        public void FriendBadgeReceived()
        {
            _eventStream.PublishAsync(new FriendBadgeReceivedEvent(_userService.CurrentUser));
        }

        [HttpGet(nameof(FriendPointsReceived))]
        public void FriendPointsReceived()
        {
            _eventStream.PublishAsync(new FriendPointsReceivedEvent(_userService.CurrentUser));
        }
    }
}