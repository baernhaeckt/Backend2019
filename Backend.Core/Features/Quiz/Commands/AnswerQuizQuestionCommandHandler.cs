using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Backend.Core.Entities.Quiz;
using Backend.Core.Events;
using Backend.Core.Features.Quiz.Models;
using Backend.Core.Features.Quiz.Shared;
using Backend.Core.Framework;
using Backend.Infrastructure.Abstraction.Persistence;
using Microsoft.Extensions.Logging;
using Silverback.Messaging.Publishing;

namespace Backend.Core.Features.Quiz.Commands
{
    public class AnswerQuizQuestionCommandHandler : CommandHandler<AnswerQuizQuestionCommand, QuestionAnswerResponse>
    {
        private readonly IEventPublisher _eventPublisher;

        public AnswerQuizQuestionCommandHandler(IUnitOfWork unitOfWork, ILogger<AnswerQuizQuestionCommandHandler> logger, IEventPublisher eventPublisher)
            : base(unitOfWork, logger) => _eventPublisher = eventPublisher;

        public override async Task<QuestionAnswerResponse> ExecuteAsync(AnswerQuizQuestionCommand command)
        {
            Logger.ExecuteAnswerQuizQuestion(command.UserId, command.QuestionAnswer.QuestionId);

            QuizQuestion question = await UnitOfWork.GetByIdOrThrowAsync<QuizQuestion>(command.QuestionAnswer.QuestionId);

            var quizService = new QuizService(UnitOfWork);
            if ((await quizService.GetUserQuizAnswerForTodayAsync(command.UserId)).Any(q => q.QuizQuestionId == command.QuestionAnswer.QuestionId))
            {
                throw new ValidationException("Question has already been answered today");
            }

            bool isCorrectAnswer = IsAnswerCorrect(question.CorrectAnswers, command.QuestionAnswer.Answers);
            var questionAnswerResponse = new QuestionAnswerResponse
            {
                IsCorrect = isCorrectAnswer,
                AwardedPoints = isCorrectAnswer ? question.Points : 0,
                DetailedAnswer = question.DetailedAnswer
            };

            await StoreAnswer(command.QuestionAnswer, questionAnswerResponse, command.UserId);
            if (isCorrectAnswer)
            {
                await _eventPublisher.PublishAsync(new QuizQuestionCorrectAnsweredEvent(command.UserId, question.Points));
            }

            Logger.ExecuteAnswerQuizQuestionSuccessful(command.UserId, command.QuestionAnswer.QuestionId);

            return questionAnswerResponse;
        }

        private async Task StoreAnswer(QuestionAnswer answer, QuestionAnswerResponse answerResponse, Guid userId)
        {
            UserQuiz userQuiz = await UnitOfWork.FirstOrDefaultAsync<UserQuiz>(uq => uq.UserId == userId);
            if (userQuiz == null)
            {
                userQuiz = new UserQuiz { UserId = userId };
                userQuiz = await UnitOfWork.InsertAsync(userQuiz);
            }

            if (!userQuiz.AnswersByDay.ContainsKey(DateTime.Today.ToString(Formats.DateKeyFormat, CultureInfo.InvariantCulture)))
            {
                userQuiz.AnswersByDay.Add(DateTime.Today.ToString(Formats.DateKeyFormat, CultureInfo.InvariantCulture), new List<UserQuizAnswer>());
            }

            userQuiz.AnswersByDay[DateTime.Today.ToString(Formats.DateKeyFormat, CultureInfo.InvariantCulture)].Add(new UserQuizAnswer
            {
                IsCorrect = answerResponse.IsCorrect,
                QuizQuestionId = answer.QuestionId,
                Points = answerResponse.AwardedPoints,
                SelectedAnswer = answer.Answers.ToList()
            });

            await UnitOfWork.UpdateAsync(userQuiz);
        }

        private static bool IsAnswerCorrect(IEnumerable<string> correctAnswers, IEnumerable<string> userAnswers)
        {
            return userAnswers.Count() == correctAnswers.Count() && correctAnswers.ToList().All(a => userAnswers.Any(uA => uA == a));
        }
    }
}