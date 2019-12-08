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

        public override async Task<IEnumerable<PointHistoryForUserQueryResult>> ExecuteAsync(PointHistoryForUserQuery query)
        {
            IEnumerable<PointAction> pointList = await Reader.GetByIdOrThrowAsync<User, IEnumerable<PointAction>>(query.Id, u => u.PointHistory.Take(MaxPointActionRecordsCount));
            return pointList.OrderByDescending(p => p.Date).Select(p => new PointHistoryForUserQueryResult(
                p.Date,
                p.Action,
                p.Point,
                p.Co2Saving,
                p.SufficientType)).ToList();
        }
    }
}