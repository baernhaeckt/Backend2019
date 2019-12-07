using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Core.Entities.Quiz;
using Backend.Core.Extensions;
using Backend.Core.Features.Quiz.Shared;
using Backend.Core.Framework;
using Backend.Infrastructure.Abstraction.Persistence;
using Microsoft.Extensions.Logging;

namespace Backend.Core.Features.Quiz.Queries
{
    internal class QuizQuestionForTodayQueryHandler : QueryHandler<QuizQuestionForTodayQueryResult, QuizQuestionForTodayQuery>
    {
        public QuizQuestionForTodayQueryHandler(IReader reader, ILogger<QuizQuestionForTodayQueryHandler> logger)
            : base(reader, logger)
        {
        }

        public override async Task<QuizQuestionForTodayQueryResult?> ExecuteAsync(QuizQuestionForTodayQuery query)
        {
            Logger.RetrieveQuizQuestionForToday(query.UserId);
            IEnumerable<QuizQuestion> questions = await Reader.GetAllAsync<QuizQuestion>();
            var quizService = new QuizService(Reader);
            IEnumerable<UserQuizAnswer> answeredQuestions = await quizService.GetUserQuizAnswerForTodayAsync(query.UserId);
            List<QuizQuestion> unansweredQuestions = questions.Where(q => answeredQuestions.All(aQ => aQ.QuizQuestionId != q.Id)).ToList();
            unansweredQuestions.Shuffle();

            QuizQuestionForTodayQueryResult result = unansweredQuestions.Select(Cast).FirstOrDefault();

            Logger.RetrieveQuizQuestionForTodaySuccessful(query.UserId, result != null);

            return result;
        }

        private static QuizQuestionForTodayQueryResult Cast(QuizQuestion quizQuestion)
        {
            List<string> allAnswers = quizQuestion.IncorrectAnswers.ToList();
            allAnswers.AddRange(quizQuestion.CorrectAnswers);
            allAnswers.Shuffle();
            return new QuizQuestionForTodayQueryResult(quizQuestion.Id, quizQuestion.Question, allAnswers);
        }
    }
}