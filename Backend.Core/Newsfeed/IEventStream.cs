using System.Threading.Tasks;

namespace Backend.Core.Newsfeed
{
    public interface IEventStream
    {
        Task PublishAsync(Event @event);
    }
}