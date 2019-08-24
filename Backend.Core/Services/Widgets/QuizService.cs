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
        private IMongoOperation<QuizQuestion> QuestionRepository { get; }
        private IMongoOperation<UserQuiz> UserQuizRepository { get; }
        public UserService UserService { get; }

        public QuizService(
                IMongoOperation<QuizQuestion> questionRepository,
                IMongoOperation<UserQuiz> userQuizRepository,
                IMongoOperation<User> userRepository,
                UserService userService,
                ClaimsPrincipal principal)
            : base(userRepository, principal)
        {
            QuestionRepository = questionRepository;
            UserQuizRepository = userQuizRepository;
            UserService = userService;
        }

        private UserQuiz CurrentUserQuiz => UserQuizRepository.GetQuerableAsync()
            .SingleOrDefault(uq => uq.UserId == CurrentUser.Id);

        public async Task<QuestionResponse> Get()
        {
            var questions = await QuestionRepository.GetAllAsync();
            var answeredQuestions = GetUserQuizAnswerForDay(DateTime.Today);
            var unansweredQuestions = questions.Where(q => answeredQuestions.All(aQ => aQ.QuizQuestionId != q.Id)).ToList();
            unansweredQuestions.Shuffle();

            return unansweredQuestions.Select(q => Cast(q)).FirstOrDefault();
        }

        public async Task<QuestionAnswerResponse> Answer(QuestionAnswer answer)
        {
            var question = QuestionRepository.GetQuerableAsync().FirstOrDefault(q => q.Id == answer.QuestionId);
            if (question == null)
            {
                throw new WebException($"question with id: {answer.QuestionId} not found.", System.Net.HttpStatusCode.NotFound);
            }
            if (GetUserQuizAnswerForDay(DateTime.Today).Any(q => q.QuizQuestionId == answer.QuestionId))
            {
                throw new WebException("question has already been answered today", System.Net.HttpStatusCode.BadRequest);
            }
            var isCorrectAnswer = IsAnswerCorrect(question.CorrectAnswers, answer.Answers);
            var questionAnswerResponse = new QuestionAnswerResponse
            {
                IsCorrect = isCorrectAnswer,
                AwardedPoints = isCorrectAnswer ? question.Points : 0,
                DetailedAnswer = question.DetailedAnswer
            };

            storeAnswer(answer, questionAnswerResponse);
            if (isCorrectAnswer)
            {
                await UserService.AddPoints(new PointAwarding
                {
                    Points = question.Points,
                    Co2Saving = 0.0,
                    Source = PointAwardingKind.Widget,
                    Text = "[Widget] Quiz eine korrekte Antwort gegeben."
                });
            }

            return questionAnswerResponse;
        }

        private void storeAnswer(QuestionAnswer answer, QuestionAnswerResponse answerResponse) 
        {
            var userQuiz = UserQuizRepository.GetQuerableAsync().FirstOrDefault(uq => uq.Id == CurrentUser.Id);
            if (userQuiz == null)
            {
                userQuiz = new UserQuiz { UserId = CurrentUser.Id };
                UserQuizRepository.SaveAsync(userQuiz);
            }

            if (!userQuiz.AnswersByDay.ContainsKey(DateTime.Today))
            {
                userQuiz.AnswersByDay.Add(DateTime.Today, new List<UserQuizAnswer>());
            }

            userQuiz.AnswersByDay[DateTime.Today].Add(new UserQuizAnswer {
                IsCorrect = answerResponse.IsCorrect,
                QuizQuestionId = answer.QuestionId,
                Points = answerResponse.AwardedPoints,
                SelectedAnswer = answer.Answers.ToList()
            });

            UserQuizRepository.UpdateAsync(userQuiz.Id, userQuiz);
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
            if (CurrentUserQuiz?.AnswersByDay.TryGetValue(dayWithoutTime, out submittedQuestionAnswer) ?? false)
            {
                return submittedQuestionAnswer;
            }

            return Enumerable.Empty<UserQuizAnswer>();
        }

        public IEnumerable<SubmittedQuestionAnswer> GetSubmittedAnswersForDay(DateTime day)
        {
            return GetUserQuizAnswerForDay(day).Select(sqa =>
            {
                var question = QuestionRepository.GetByIdAsync(sqa.QuizQuestionId).Result;

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

        public void Insert(QuizQuestion question)
        {
            QuestionRepository.SaveAsync(question);
        }
    }
}
