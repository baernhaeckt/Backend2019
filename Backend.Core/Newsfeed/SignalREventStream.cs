using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Backend.Core.Newsfeed
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
                await _hubContext.Clients.Group(user).SendAsync("newEvent", @event);
            }
        }
    }
}