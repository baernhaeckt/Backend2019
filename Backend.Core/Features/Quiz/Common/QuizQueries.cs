using System;
using System.Linq;
using System.Threading.Tasks;
using Backend.Core.Entities.Quiz;
using Backend.Infrastructure.Abstraction.Hosting;
using Backend.Infrastructure.Abstraction.Persistence;

namespace Backend.Core.Features.Quiz.Common
{
    public static class QuizQueries
    {
        public static async Task<long> GetAnsweredToday(IReader reader, Guid userId, IClock clock)
        {
            DateTimeOffset now = clock.Now();
            var start = new DateTimeOffset(now.Year, now.Month, now.Day, 0, 0, 0, 0, new TimeSpan(0));
            var end = new DateTimeOffset(now.Year, now.Month, now.Day, 23, 59, 59, 999, new TimeSpan(0));
            return await reader.CountAsync<Question>(q => q.AnsweredBy.Any(a => a.UserId == userId && a.AnsweredAt <= end && a.AnsweredAt >= start));
        }
    }
}
