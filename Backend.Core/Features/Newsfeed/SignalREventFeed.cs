using System;
using System.Threading.Tasks;
using Backend.Core.Features.Newsfeed.Abstraction;
using Backend.Core.Features.Newsfeed.Events;
using Backend.Core.Features.Newsfeed.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Backend.Core.Features.Newsfeed
{
    public class SignalREventFeed : IEventFeed
    {
        private readonly IHubContext<NewsfeedHub> _hubContext;

        private readonly ILogger<SignalREventFeed> _logger;

        public SignalREventFeed(IHubContext<NewsfeedHub> hubContext, ILogger<SignalREventFeed> logger)
        {
            _hubContext = hubContext;
            _logger = logger;
        }

        public async Task PublishAsync(NewsfeedEvent newsfeedEvent)
        {
            _logger.PublishEventToNewsFeed(newsfeedEvent);

            foreach (Guid user in newsfeedEvent.Audience)
            {
                await _hubContext.Clients.Group(user.ToString()).SendAsync("newEvent", newsfeedEvent);
            }
        }
    }
}