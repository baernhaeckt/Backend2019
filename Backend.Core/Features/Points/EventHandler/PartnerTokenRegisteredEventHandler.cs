using System.Threading.Tasks;
using Backend.Core.Events;
using Silverback.Messaging.Subscribers;

namespace Backend.Core.Features.Points.EventHandler
{
    internal class PartnerTokenRegisteredEventHandler : ISubscriber
    {
        private readonly PointService _pointService;

        public PartnerTokenRegisteredEventHandler(PointService pointService)
        {
            _pointService = pointService;
        }

        public async Task ExecuteAsync(PartnerTokenRegisteredEvent command)
        {
            await _pointService.AddPoints(command.Token);
        }
    }
}