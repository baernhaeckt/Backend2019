using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNetCore.MongoDB;
using Backend.Database;
using Backend.Database.Widgets.Quiz;
using Backend.Models;
using Backend.Models.Widgets.Quiz;

namespace Backend.Core.Services.Widgets
{
    public class QuizService : PersonalizedService, IQuizService
    {
        private const string DateKeyFormat = "yyyyMMdd";

        private IMongoOperation<QuizQuestion> _questionRepository;

        private IMongoOperation<UserQuiz> _userQuizRepository;

        private UserService _userService;

        public QuizService(
                IMongoOperation<QuizQuestion> questionRepository,
                IMongoOperation<UserQuiz> userQuizRepository,
                IMongoOperation<User> userRepository,
                UserService userService,
                ClaimsPrincipal principal)
            : base(userRepository, principal)
        {
            _questionRepository = questionRepository;
            _userQuizRepository = userQuizRepository;
            _userService = userService;
        }

        private UserQuiz CurrentUserQuiz => _userQuizRepository.GetQuerableAsync()
            .SingleOrDefault(uq => uq.UserId == CurrentUser.Id);

        public async Task<QuestionResponse> Get()
        {
            var questions = await _questionRepository.GetAllAsync();
            var answeredQuestions = GetUserQuizAnswerForDay(DateTime.Today);
            var unansweredQuestions = questions.Where(q => answeredQuestions.All(aQ => aQ.QuizQuestionId != q.Id)).ToList();
            unansweredQuestions.Shuffle();

            return unansweredQuestions.Select(q => Cast(q)).FirstOrDefault();
        }

        public async Task<QuestionAnswerResponse> Answer(QuestionAnswer answer)
        {
            var question = _questionRepository.GetQuerableAsync().FirstOrDefault(q => q.Id == answer.QuestionId);
            if (question == null)
            {
                throw new WebException($"Question with id: {answer.QuestionId} not found.", System.Net.HttpStatusCode.NotFound);
            }
            if (GetUserQuizAnswerForDay(DateTime.Today).Any(q => q.QuizQuestionId == answer.QuestionId))
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
                await _userService.AddPoints(new PointAwarding
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
            var userQuiz = _userQuizRepository.GetQuerableAsync().FirstOrDefault(uq => uq.UserId == CurrentUser.Id);
            if (userQuiz == null)
            {
                userQuiz = new UserQuiz { UserId = CurrentUser.Id };
                userQuiz = await _userQuizRepository.SaveAsync(userQuiz);
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

            await _userQuizRepository.UpdateAsync(userQuiz.Id, userQuiz);
        }

        private bool IsAnswerCorrect(IEnumerable<string> correctAnswers, IEnumerable<string> userAnswers)
        {
            if (userAnswers.Count() != correctAnswers.Count())
            {
                return false;
            }

            return correctAnswers.ToList().All(a => userAnswers.Any(uA => uA == a));
        }

        public IEnumerable<SubmittedQuestionAnswer> SubmittedAnswersForToday
            => GetSubmittedAnswersForDay(DateTime.Today);

        private IEnumerable<UserQuizAnswer> GetUserQuizAnswerForDay(DateTime day)
        {
            var dayWithoutTime = new DateTime(day.Year, day.Month, day.Day);
            IList<UserQuizAnswer> submittedQuestionAnswer = null;
            if (CurrentUserQuiz?.AnswersByDay.TryGetValue(dayWithoutTime.ToString(DateKeyFormat), out submittedQuestionAnswer) ?? false)
            {
                return submittedQuestionAnswer;
            }

            return Enumerable.Empty<UserQuizAnswer>();
        }

        public IEnumerable<SubmittedQuestionAnswer> GetSubmittedAnswersForDay(DateTime day)
        {
            return GetUserQuizAnswerForDay(day).Select(sqa =>
            {
                var question = _questionRepository.GetByIdAsync(sqa.QuizQuestionId).Result;

                return new SubmittedQuestionAnswer
                {
                    IsCorrect = sqa.IsCorrect,
                    Points = sqa.Points,
                    Question = Cast(question),
                    SelectedAnswers = sqa.SelectedAnswer,
                    DetailedAnswer = question.DetailedAnswer
                };
            });
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
