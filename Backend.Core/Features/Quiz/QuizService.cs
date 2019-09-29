using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Backend.Core.Extensions;
using Backend.Core.Features.PointsAndAwards;
using Backend.Core.Features.PointsAndAwards.Models;
using Backend.Core.Features.Quiz.Models;
using Backend.Database.Abstraction;
using Backend.Database.Widgets.Quiz;

namespace Backend.Core.Features.Quiz
{
    public class QuizService
    {
        private const string DateKeyFormat = "yyyyMMdd";
        private readonly IUnitOfWork _unitOfWork;
        private readonly PointService _pointService;
        private readonly ClaimsPrincipal _principal;

        public QuizService(IUnitOfWork unitOfWork, PointService pointService, ClaimsPrincipal principal)
        {
            _unitOfWork = unitOfWork;
            _pointService = pointService;
            _principal = principal;
        }

        public async Task<QuestionResponse> Get()
        {
            var questions = await _unitOfWork.GetAllAsync<QuizQuestion>();
            var answeredQuestions = await GetUserQuizAnswerForDayAsync(DateTime.Today);
            var unansweredQuestions = questions.Where(q => answeredQuestions.All(aQ => aQ.QuizQuestionId != q.Id)).ToList();
            unansweredQuestions.Shuffle();

            return unansweredQuestions.Select(q => Cast(q)).FirstOrDefault();
        }

        public async Task<QuestionAnswerResponse> Answer(QuestionAnswer answer)
        {
            var question = await _unitOfWork.GetAsync<QuizQuestion>(answer.QuestionId);
            if (question == null)
            {
                throw new WebException($"Question with id: {answer.QuestionId} not found.", System.Net.HttpStatusCode.NotFound);
            }

            if ((await GetUserQuizAnswerForDayAsync(DateTime.Today)).Any(q => q.QuizQuestionId == answer.QuestionId))
            {
                throw new WebException("Question has already been answered today", System.Net.HttpStatusCode.BadRequest);
            }

            var isCorrectAnswer = IsAnswerCorrect(question.CorrectAnswers, answer.Answers);
            var questionAnswerResponse = new QuestionAnswerResponse
            {
                IsCorrect = isCorrectAnswer,
                AwardedPoints = isCorrectAnswer ? question.Points : 0,
                DetailedAnswer = question.DetailedAnswer
            };

            await StoreAnswer(answer, questionAnswerResponse);
            if (isCorrectAnswer)
            {
                await _pointService.AddPoints(new PointAwarding
                {
                    Points = question.Points,
                    Co2Saving = 0.0,
                    Source = PointAwardingKind.Widget,
                    Text = "[Widget] Quiz eine korrekte Antwort gegeben."
                });
            }

            return questionAnswerResponse;
        }

        private async Task StoreAnswer(QuestionAnswer answer, QuestionAnswerResponse answerResponse)
        {
            var userQuiz = await _unitOfWork.FirstOrDefaultAsync<UserQuiz>(uq => uq.UserId == _principal.Id());
            if (userQuiz == null)
            {
                userQuiz = new UserQuiz { UserId = _principal.Id() };
                userQuiz = await _unitOfWork.InsertAsync(userQuiz);
            }

            if (!userQuiz.AnswersByDay.ContainsKey(DateTime.Today.ToString(DateKeyFormat)))
            {
                userQuiz.AnswersByDay.Add(DateTime.Today.ToString(DateKeyFormat), new List<UserQuizAnswer>());
            }

            userQuiz.AnswersByDay[DateTime.Today.ToString(DateKeyFormat)].Add(new UserQuizAnswer
            {
                IsCorrect = answerResponse.IsCorrect,
                QuizQuestionId = answer.QuestionId,
                Points = answerResponse.AwardedPoints,
                SelectedAnswer = answer.Answers.ToList()
            });

            await _unitOfWork.UpdateAsync(userQuiz);
        }

        private bool IsAnswerCorrect(IEnumerable<string> correctAnswers, IEnumerable<string> userAnswers)
        {
            if (userAnswers.Count() != correctAnswers.Count())
            {
                return false;
            }

            return correctAnswers.ToList().All(a => userAnswers.Any(uA => uA == a));
        }

        private async Task<IEnumerable<UserQuizAnswer>> GetUserQuizAnswerForDayAsync(DateTime day)
        {
            var dayWithoutTime = new DateTime(day.Year, day.Month, day.Day);
            IList<UserQuizAnswer> submittedQuestionAnswer = null;
            UserQuiz currentUserQuiz = await _unitOfWork.SingleOrDefaultAsync<UserQuiz>(uq => uq.UserId == _principal.Id());
            if (currentUserQuiz?.AnswersByDay.TryGetValue(dayWithoutTime.ToString(DateKeyFormat), out submittedQuestionAnswer) ?? false)
            {
                return submittedQuestionAnswer;
            }

            return Enumerable.Empty<UserQuizAnswer>();
        }

        public async Task<IEnumerable<SubmittedQuestionAnswer>> GetSubmittedAnswersForDayAsync(DateTime day)
        {
            IList<SubmittedQuestionAnswer> submittedQuestionAnswers = new List<SubmittedQuestionAnswer>();

            IEnumerable<UserQuizAnswer> userQuizAnswers = await GetUserQuizAnswerForDayAsync(day);
            foreach (var userQuizAnswer in userQuizAnswers)
            {
                var question = await _unitOfWork.GetAsync<QuizQuestion>(userQuizAnswer.QuizQuestionId);

                submittedQuestionAnswers.Add(new SubmittedQuestionAnswer
                {
                    IsCorrect = userQuizAnswer.IsCorrect,
                    Points = userQuizAnswer.Points,
                    Question = Cast(question),
                    SelectedAnswers = userQuizAnswer.SelectedAnswer,
                    DetailedAnswer = question.DetailedAnswer
                });
            }

            return submittedQuestionAnswers;
        }

        private QuestionResponse Cast(QuizQuestion quizQuestion)
        {
            var allAnswers = quizQuestion.IncorrectAnswers.ToList();
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
