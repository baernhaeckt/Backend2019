using System.Threading.Tasks;
using Backend.Core.Features.Newsfeed.Events;
using Microsoft.AspNetCore.SignalR;

namespace Backend.Core.Features.Newsfeed
{
    public class SignalREventStream : IEventStream
    {
        private readonly IHubContext<NewsfeedHub> _hubContext;

        public SignalREventStream(IHubContext<NewsfeedHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task PublishAsync(Event @event)
        {
            foreach (var user in @event.Audience)
            {
                await _hubContext.Clients.Group(user.ToString()).SendAsync("newEvent", @event);
            }
        }
    }
}