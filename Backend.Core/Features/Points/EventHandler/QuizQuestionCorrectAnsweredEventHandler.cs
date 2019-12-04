using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Core.Entities;
using Backend.Core.Events;
using Backend.Infrastructure.Abstraction.Persistence;
using Microsoft.Extensions.Logging;
using Silverback.Messaging.Publishing;
using Silverback.Messaging.Subscribers;

namespace Backend.Core.Features.Points.EventHandler
{
    internal class QuizQuestionCorrectAnsweredEventHandler : ISubscriber
    {
        private readonly IEventPublisher _eventPublisher;

        private readonly ILogger<QuizQuestionCorrectAnsweredEventHandler> _logger;

        private readonly IUnitOfWork _unitOfWork;

        public QuizQuestionCorrectAnsweredEventHandler(IUnitOfWork unitOfWork, IEventPublisher eventPublisher, ILogger<QuizQuestionCorrectAnsweredEventHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _eventPublisher = eventPublisher;
            _logger = logger;
        }

        public async Task ExecuteAsync(QuizQuestionCorrectAnsweredEvent @event)
        {
            _logger.PartnerInitiateGrantPointsForCorrectQuizAnswer(@event.UserId, @event.QuestionPoints);

            (Guid id, int points) = await _unitOfWork.GetByIdOrThrowAsync<User, Tuple<Guid, int>>(@event.UserId, u => new Tuple<Guid, int>(u.Id, u.PointHistory.Sum(pa => pa.Point)));

            var updateObject = new
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
            };

            _logger.PartnerGrantPointsForCorrectQuizAnswerUpdateUser(updateObject);
            await _unitOfWork.UpdateAsync<User>(id, updateObject);

            // TODO: Refactor this for performance reasons.
            User user = await _unitOfWork.GetByIdOrDefaultAsync<User>(@event.UserId);
            await _eventPublisher.PublishAsync(new UserNewPointsEvent(user, @event.QuestionPoints, 0));

            _logger.PartnerSuccessfulGrantPointsForCorrectQuizAnswer(@event.UserId, @event.QuestionPoints);
        }
    }
}