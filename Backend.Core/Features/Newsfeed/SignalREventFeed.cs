using System;
using System.Threading.Tasks;
using Backend.Core.Features.Newsfeed.Abstraction;
using Backend.Core.Features.Newsfeed.Events;
using Backend.Core.Features.Newsfeed.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace Backend.Core.Features.Newsfeed
{
    public class SignalREventFeed : IEventFeed
    {
        private readonly IHubContext<NewsfeedHub> _hubContext;

        public SignalREventFeed(IHubContext<NewsfeedHub> hubContext) => _hubContext = hubContext;

        public async Task PublishAsync(NewsfeedEvent newsfeedEvent)
        {
            foreach (Guid user in newsfeedEvent.Audience)
            {
                await _hubContext.Clients.Group(user.ToString()).SendAsync("newEvent", newsfeedEvent);
            }
        }
    }
}