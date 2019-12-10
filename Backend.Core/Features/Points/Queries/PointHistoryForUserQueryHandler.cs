using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Core.Entities;
using Backend.Core.Framework;
using Backend.Infrastructure.Abstraction.Persistence;
using Microsoft.Extensions.Logging;

namespace Backend.Core.Features.Points.Queries
{
    internal class PointHistoryForUserQueryHandler : QueryHandler<IEnumerable<PointHistoryForUserQueryResult>, PointHistoryForUserQuery>
    {
        private const int MaxPointActionRecordsCount = 25;

        public PointHistoryForUserQueryHandler(IReader reader, ILogger<PointHistoryForUserQueryHandler> logger)
            : base(reader, logger)
        {
        }

        public override Task<IEnumerable<PointHistoryForUserQueryResult>> ExecuteAsync(PointHistoryForUserQuery query)
        {
            Logger.RetrievePointHistoryForUser(query.Id);

            IQueryable<User> queryable = Reader.GetQueryable<User>();

            IQueryable<PointHistoryForUserQueryResult> result = queryable
                .Where(u => u.Id == query.Id)
                .SelectMany(u => u.PointHistory)
                .OrderByDescending(p => p.Date)
                .Select(p => new PointHistoryForUserQueryResult(p.Date, p.Action, p.Point, p.Co2Saving, p.Type))
                .Take(MaxPointActionRecordsCount);

            Logger.RetrievePointHistoryForUserSuccessful(query.Id);

            return Task.FromResult(result.AsEnumerable());
        }
    }
}