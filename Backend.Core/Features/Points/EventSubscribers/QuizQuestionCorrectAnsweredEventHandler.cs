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

namespace Backend.Core.Features.Points.EventSubscribers
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
            _logger.HandleQuizQuestionCorrectAnsweredEvent(@event.UserId, @event.QuestionPoints);

            // Use PointHistory to get the current points and take this as a chance to sync, if there is a mismatch between the Points and PointHistory.
            (Guid id, string displayName, int points) = await _unitOfWork.GetByIdOrThrowAsync<User, Tuple<Guid, string, int>>
                (@event.UserId, u => new Tuple<Guid, string, int>(u.Id, u.DisplayName, u.PointHistory.Sum(pa => pa.Point)));

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

            await _eventPublisher.PublishAsync(new UserNewPointsEvent(@event.UserId, @event.QuestionPoints, 0, displayName));

            _logger.HandleQuizQuestionCorrectAnsweredEventSuccessful(@event.UserId, @event.QuestionPoints);
        }
    }
}