using System.Threading.Tasks;
using Backend.Core.Events;
using Backend.Core.Features.Points.Models;
using Silverback.Messaging.Subscribers;

namespace Backend.Core.Features.Points.EventHandler
{
    internal class QuizQuestionCorrectAnsweredEventHandler : ISubscriber
    {
        private readonly PointService _pointService;

        public QuizQuestionCorrectAnsweredEventHandler(PointService pointService) => _pointService = pointService;

        public async Task ExecuteAsync(QuizQuestionCorrectAnsweredEvent @event)
        {
            await _pointService.AddPoints(new PointAwarding
            {
                Points = @event.QuestionPoints,
                Co2Saving = 0.0,
                Source = PointAwardingKind.Widget,
                Text = "[Widget] Quiz eine korrekte Antwort gegeben."
            });
        }
    }
}