using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Backend.Core.Entities.Quiz;
using Backend.Core.Events;
using Backend.Core.Features.Quiz.Common;
using Backend.Core.Framework;
using Backend.Infrastructure.Abstraction.Hosting;
using Backend.Infrastructure.Abstraction.Persistence;
using Microsoft.Extensions.Logging;
using Silverback.Messaging.Publishing;

namespace Backend.Core.Features.Quiz.Commands
{
    public class AnswerQuizQuestionCommandHandler : CommandHandler<AnswerQuizQuestionCommand, AnswerQuizQuestionResult>
    {
        private readonly IEventPublisher _eventPublisher;

        private readonly IClock _clock;

        public AnswerQuizQuestionCommandHandler(IUnitOfWork unitOfWork, ILogger<AnswerQuizQuestionCommandHandler> logger, IEventPublisher eventPublisher, IClock clock)
            : base(unitOfWork, logger)
        {
            _eventPublisher = eventPublisher;
            _clock = clock;
        }

        public override async Task<AnswerQuizQuestionResult> ExecuteAsync(AnswerQuizQuestionCommand command)
        {
            Logger.ExecuteAnswerQuizQuestion(command.UserId, command.QuestionId);

            long answeredToday = await QuizQueries.GetAnsweredToday(UnitOfWork, command.UserId, _clock);
            if (answeredToday >= Constants.MaxQuestionsPerDay)
            {
                throw new ValidationException("All questions for today already answered.");
            }

            long alreadyAnsweredCount = await UnitOfWork.CountAsync<Question>(q => q.Id == command.QuestionId && q.AnsweredBy.Any(a => a.UserId == command.UserId));
            if (alreadyAnsweredCount >= 1)
            {
                throw new ValidationException("Question has already been answered.");
            }

            var question = await UnitOfWork.SingleAsync<Question, Question>(q => q.Id == command.QuestionId, q => new Question
            {
                Answers = q.Answers,
                ExplanationText = q.ExplanationText,
                Points = q.Points
            });
            bool isCorrectAnswer = question.Answers.Single(a => a.Id == command.AnswerId).IsCorrect;
            var questionAnswerResponse = new AnswerQuizQuestionResult
            {
                IsCorrect = isCorrectAnswer,
                AwardedPoints = isCorrectAnswer ? question.Points : 0,
                DetailedAnswer = question.ExplanationText.GetForCurrentCulture()
            };

            await UnitOfWork.UpdateAsync<Question>(command.QuestionId, new
            {
                AnsweredBy = new List<Answered>
                {
                    new Answered
                    {
                        UserId = command.UserId,
                        AnsweredAt = _clock.Now(),
                        AnswerId = command.AnswerId
                    }
                }
            });

            if (isCorrectAnswer)
            {
                await _eventPublisher.PublishAsync(new QuizQuestionCorrectAnsweredEvent(command.UserId, question.Points));
            }

            Logger.ExecuteAnswerQuizQuestionSuccessful(command.UserId, command.QuestionId, isCorrectAnswer, command.AnswerId);

            return questionAnswerResponse;
        }
    }
}