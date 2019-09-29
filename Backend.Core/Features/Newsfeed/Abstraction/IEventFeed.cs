using System.Threading.Tasks;
using Backend.Core.Features.Newsfeed.Events;

namespace Backend.Core.Features.Newsfeed.Abstraction
{
    public interface IEventFeed
    {
        Task PublishAsync(NewsfeedEvent newsfeedEvent);
    }
}