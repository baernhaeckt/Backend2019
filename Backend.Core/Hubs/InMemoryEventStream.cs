using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Backend.Core.Hubs
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
           await _hubContext.Clients.All.SendAsync("newEvent", @event);
        }
    }
}