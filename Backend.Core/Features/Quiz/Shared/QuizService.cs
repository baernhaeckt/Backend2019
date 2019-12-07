using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Backend.Core.Entities.Quiz;
using Backend.Infrastructure.Abstraction.Persistence;

namespace Backend.Core.Features.Quiz.Shared
{
    [Obsolete("Will be refactored away.")]
    public class QuizService
    {
        private readonly IReader _reader;

        public QuizService(IReader reader) => _reader = reader;

        public async Task<IEnumerable<UserQuizAnswer>> GetUserQuizAnswerForTodayAsync(Guid userId)
        {
            var day = DateTime.Today;
            var dayWithoutTime = new DateTime(day.Year, day.Month, day.Day);
            UserQuiz currentUserQuiz = await _reader.SingleOrDefaultAsync<UserQuiz>(uq => uq.UserId == userId);
            if (currentUserQuiz != null)
            {
                string dayWithoutTimeString = dayWithoutTime.ToString(Formats.DateKeyFormat, CultureInfo.InvariantCulture);
                currentUserQuiz.AnswersByDay.TryGetValue(dayWithoutTimeString, out IList<UserQuizAnswer>? submittedQuestionAnswer);
                return submittedQuestionAnswer ?? Enumerable.Empty<UserQuizAnswer>();
            }

            return Enumerable.Empty<UserQuizAnswer>();
        }
    }
}