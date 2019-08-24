using System.Threading.Tasks;

namespace Backend.Core.Hubs
{
    public interface IEventStream
    {
        Task PublishAsync(Event @event);
    }
}