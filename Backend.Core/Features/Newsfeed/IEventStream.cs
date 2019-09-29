using System.Threading.Tasks;
using Backend.Core.Features.Newsfeed.Events;

namespace Backend.Core.Features.Newsfeed
{
    public interface IEventStream
    {
        Task PublishAsync(Event @event);
    }
}