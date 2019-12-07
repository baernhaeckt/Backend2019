using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Backend.Core.Entities.Quiz;
using Backend.Core.Extensions;
using Backend.Core.Features.Quiz.Common;
using Backend.Core.Framework;
using Backend.Infrastructure.Abstraction.Hosting;
using Backend.Infrastructure.Abstraction.Persistence;
using Microsoft.Extensions.Logging;

namespace Backend.Core.Features.Quiz.Queries
{
    internal class QuizQuestionForTodayQueryHandler : QueryHandler<QuizQuestionForTodayQueryResult?, QuizQuestionForTodayQuery>
    {
        private readonly IClock _clock;

        public QuizQuestionForTodayQueryHandler(IReader reader, ILogger<QuizQuestionForTodayQueryHandler> logger, IClock clock)
            : base(reader, logger)
        {
            _clock = clock;
        }

        [SuppressMessage("ReSharper", "SimplifyLinqExpression", Justification = "MongoDb driver doesn't support 'All'.")]
        public override async Task<QuizQuestionForTodayQueryResult?> ExecuteAsync(QuizQuestionForTodayQuery query)
        {
            Logger.RetrieveQuizQuestionForToday(query.UserId);

            // For the future: Maybe firs t limit by time (e.g. only look at questions created in the past few weeks)
            // Maybe retrieve and cache top(x) where x is the questions answerable by day, as we suppose that the user will answer them in a row.
            IEnumerable<Question> questions = await Reader.WhereAsync<Question, Question>(
                q => !q.AnsweredBy.Any(a => a.UserId == query.UserId),
                q => new Question
                {
                    Id = q.Id,
                    Answers = q.Answers,
                    QuestionText = q.QuestionText
                });

            long answeredToday = await QuizQueries.GetAnsweredToday(Reader, query.UserId, _clock);

            QuizQuestionForTodayQueryResult? result = null;
            if (answeredToday < Constants.MaxQuestionsPerDay)
            {
                Question question = questions.Shuffle().First();
                IEnumerable<KeyValuePair<Guid, string>> answers = question.Answers.Select(a => new KeyValuePair<Guid, string>(a.Id, a.AnswerText.GetForCurrentCulture())).Shuffle();
                result = new QuizQuestionForTodayQueryResult(question.Id, question.QuestionText.GetForCurrentCulture(), answers);
            }

            Logger.RetrieveQuizQuestionForTodaySuccessful(query.UserId, result != null, answeredToday);

            return result;
        }
    }
}