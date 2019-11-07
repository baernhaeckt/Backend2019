using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Core.Entities;
using Backend.Core.Events;
using Backend.Infrastructure.Persistence.Abstraction;
using Silverback.Messaging.Publishing;
using Silverback.Messaging.Subscribers;

namespace Backend.Core.Features.Points.EventHandler
{
    internal class QuizQuestionCorrectAnsweredEventHandler : ISubscriber
    {
        private readonly IEventPublisher _eventPublisher;

        private readonly IUnitOfWork _unitOfWork;

        public QuizQuestionCorrectAnsweredEventHandler(IUnitOfWork unitOfWork, IEventPublisher eventPublisher)
        {
            _unitOfWork = unitOfWork;
            _eventPublisher = eventPublisher;
        }

        public async Task ExecuteAsync(QuizQuestionCorrectAnsweredEvent @event)
        {
            (Guid id, int points) = await _unitOfWork.GetByIdOrThrowAsync<User, Tuple<Guid, int>>(@event.UserId, u => new Tuple<Guid, int>(u.Id, u.Points));

            await _unitOfWork.UpdateAsync<User>(id, new
            {
                Points = points + @event.QuestionPoints,
                PointHistory = new List<PointAction>
                {
                    new PointAction
                    {
                        Action = "Hat eine korrekte Quizantwort gegeben.",
                        Co2Saving = 0.0,
                        Point = @event.QuestionPoints,
                        SponsorRef = "Quiz",
                        SufficientType = new UserSufficientType { Title = "Wissen" }
                    }
                }
            });

            // TODO: Refactor this for performance reasons.
            User user = await _unitOfWork.GetByIdOrDefaultAsync<User>(@event.UserId);
            await _eventPublisher.PublishAsync(new UserNewPointsEvent(user, @event.QuestionPoints, 0));
        }
    }
}