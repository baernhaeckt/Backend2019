using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Backend.Core.Entities.Quiz;
using Backend.Core.Events;
using Backend.Core.Extensions;
using Backend.Core.Features.Quiz.Models;
using Backend.Infrastructure.Persistence.Abstraction;
using Silverback.Messaging.Publishing;

namespace Backend.Core.Features.Quiz
{
    public class QuizService
    {
        private const string DateKeyFormat = "yyyyMMdd";

        private readonly ClaimsPrincipal _principal;

        private readonly IEventPublisher _eventPublisher;

        private readonly IUnitOfWork _unitOfWork;

        public QuizService(IUnitOfWork unitOfWork, ClaimsPrincipal principal, IEventPublisher eventPublisher)
        {
            _unitOfWork = unitOfWork;
            _principal = principal;
            _eventPublisher = eventPublisher;
        }

        public async Task<QuestionResponse> Get()
        {
            IEnumerable<QuizQuestion> questions = await _unitOfWork.GetAllAsync<QuizQuestion>();
            IEnumerable<UserQuizAnswer> answeredQuestions = await GetUserQuizAnswerForDayAsync(DateTime.Today);
            List<QuizQuestion> unansweredQuestions = questions.Where(q => answeredQuestions.All(aQ => aQ.QuizQuestionId != q.Id)).ToList();
            unansweredQuestions.Shuffle();

            return unansweredQuestions.Select(Cast).FirstOrDefault();
        }

        public async Task<QuestionAnswerResponse> Answer(QuestionAnswer answer)
        {
            QuizQuestion question = await _unitOfWork.GetByIdOrThrowAsync<QuizQuestion>(answer.QuestionId);

            if ((await GetUserQuizAnswerForDayAsync(DateTime.Today)).Any(q => q.QuizQuestionId == answer.QuestionId))
            {
                throw new ValidationException("Question has already been answered today");
            }

            bool isCorrectAnswer = IsAnswerCorrect(question.CorrectAnswers, answer.Answers);
            var questionAnswerResponse = new QuestionAnswerResponse
            {
                IsCorrect = isCorrectAnswer,
                AwardedPoints = isCorrectAnswer ? question.Points : 0,
                DetailedAnswer = question.DetailedAnswer
            };

            await StoreAnswer(answer, questionAnswerResponse);
            if (isCorrectAnswer)
            {
                await _eventPublisher.PublishAsync(new QuizQuestionCorrectAnsweredEvent(question.Points));
            }

            return questionAnswerResponse;
        }

        private async Task StoreAnswer(QuestionAnswer answer, QuestionAnswerResponse answerResponse)
        {
            UserQuiz userQuiz = await _unitOfWork.FirstOrDefaultAsync<UserQuiz>(uq => uq.UserId == _principal.Id());
            if (userQuiz == null)
            {
                userQuiz = new UserQuiz { UserId = _principal.Id() };
                userQuiz = await _unitOfWork.InsertAsync(userQuiz);
            }

            if (!userQuiz.AnswersByDay.ContainsKey(DateTime.Today.ToString(DateKeyFormat, CultureInfo.InvariantCulture)))
            {
                userQuiz.AnswersByDay.Add(DateTime.Today.ToString(DateKeyFormat, CultureInfo.InvariantCulture), new List<UserQuizAnswer>());
            }

            userQuiz.AnswersByDay[DateTime.Today.ToString(DateKeyFormat, CultureInfo.InvariantCulture)].Add(new UserQuizAnswer
            {
                IsCorrect = answerResponse.IsCorrect,
                QuizQuestionId = answer.QuestionId,
                Points = answerResponse.AwardedPoints,
                SelectedAnswer = answer.Answers.ToList()
            });

            await _unitOfWork.UpdateAsync(userQuiz);
        }

        private static bool IsAnswerCorrect(IEnumerable<string> correctAnswers, IEnumerable<string> userAnswers)
        {
            return userAnswers.Count() == correctAnswers.Count() && correctAnswers.ToList().All(a => userAnswers.Any(uA => uA == a));
        }

        private async Task<IEnumerable<UserQuizAnswer>> GetUserQuizAnswerForDayAsync(DateTime day)
        {
            var dayWithoutTime = new DateTime(day.Year, day.Month, day.Day);
            UserQuiz currentUserQuiz = await _unitOfWork.SingleOrDefaultAsync<UserQuiz>(uq => uq.UserId == _principal.Id());
            if (currentUserQuiz != null)
            {
                string dayWithoutTimeString = dayWithoutTime.ToString(DateKeyFormat, CultureInfo.InvariantCulture);
                currentUserQuiz.AnswersByDay.TryGetValue(dayWithoutTimeString, out IList<UserQuizAnswer>? submittedQuestionAnswer);
                return submittedQuestionAnswer ?? Enumerable.Empty<UserQuizAnswer>();
            }

            return Enumerable.Empty<UserQuizAnswer>();
        }

        private static QuestionResponse Cast(QuizQuestion quizQuestion)
        {
            List<string> allAnswers = quizQuestion.IncorrectAnswers.ToList();
            allAnswers.AddRange(quizQuestion.CorrectAnswers);
            allAnswers.Shuffle();

            return new QuestionResponse
            {
                Id = quizQuestion.Id,
                Question = quizQuestion.Question,
                Answers = allAnswers
            };
        }
    }
}