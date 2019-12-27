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
    internal class AveragePointsPerSufficientTypesQueryHandler : QueryHandler<IEnumerable<SufficientTypesQueryResult>, AveragePointsPerSufficientTypesQuery>
    {
        public AveragePointsPerSufficientTypesQueryHandler(IReader reader, ILogger<AveragePointsPerSufficientTypesQueryHandler> logger)
            : base(reader, logger)
        {
        }

        public override Task<IEnumerable<SufficientTypesQueryResult>> ExecuteAsync(AveragePointsPerSufficientTypesQuery query)
        {
            Logger.RetrieveAllSufficientTypes();

            IQueryable<SufficientTypesQueryResult> queryResult = Reader.GetQueryable<User>()
                .SelectMany(u => u.PointHistory)
                .GroupBy(p => p.Type)
                .Select(p => new SufficientTypesQueryResult(
                    p.Key,
                    p.Average(o => o.Point),
                    p.Average(c => c.Co2Saving)));

            Task<IEnumerable<SufficientTypesQueryResult>> result = Task.FromResult(queryResult.AsEnumerable());

            Logger.RetrieveAllSufficientTypesSuccessful();

            return result;
        }
    }
}