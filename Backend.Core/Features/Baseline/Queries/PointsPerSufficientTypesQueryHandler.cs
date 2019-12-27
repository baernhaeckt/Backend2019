using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Core.Entities;
using Backend.Core.Framework;
using Backend.Core.Framework.Cqrs;
using Backend.Infrastructure.Abstraction.Persistence;
using Microsoft.Extensions.Logging;

namespace Backend.Core.Features.Baseline.Queries
{
    internal class PointsPerSufficientTypesQueryHandler : QueryHandler<IEnumerable<SufficientTypesQueryResult>, PointsPerSufficientTypesQuery>
    {
        public PointsPerSufficientTypesQueryHandler(IReader reader, ILogger<PointsPerSufficientTypesQueryHandler> logger)
            : base(reader, logger)
        {
        }

        public override Task<IEnumerable<SufficientTypesQueryResult>> ExecuteAsync(PointsPerSufficientTypesQuery query)
        {
            Logger.RetrievePointsPerSufficientTypes(query.UserId);

            IQueryable<SufficientTypesQueryResult> queryResult = Reader.GetQueryable<User>()
                .Where(u => u.Id == query.UserId)
                .SelectMany(u => u.PointHistory)
                .GroupBy(p => p.Type)
                .Select(p => new SufficientTypesQueryResult(
                    p.Key,
                    p.Sum(o => o.Point),
                    p.Sum(c => c.Co2Saving)));

            Task<IEnumerable<SufficientTypesQueryResult>> result = Task.FromResult(queryResult.AsEnumerable());

            Logger.RetrievePointsPerSufficientTypesSuccessful(query.UserId);

            return result;
        }
    }
}