using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Core.Entities;
using Backend.Core.Framework;
using Backend.Infrastructure.Abstraction.Persistence;
using Microsoft.Extensions.Logging;

namespace Backend.Core.Features.Baseline.Queries
{
    internal class PointsPerSufficientTypesQueryHandler : QueryHandler<IEnumerable<UserSufficientType>, PointsPerSufficientTypesQuery>
    {
        public PointsPerSufficientTypesQueryHandler(IReader reader, ILogger<PointsPerSufficientTypesQueryHandler> logger)
            : base(reader, logger)
        {
        }

        public override Task<IEnumerable<UserSufficientType>> ExecuteAsync(PointsPerSufficientTypesQuery query)
        {
            Logger.RetrievePointsPerSufficientTypes(query.UserId);

            IQueryable<UserSufficientType> userSufficientTypes = Reader.GetQueryable<User>()
                .Where(u => u.Id == query.UserId)
                .SelectMany(u => u.PointHistory)
                .GroupBy(p => p.SufficientType.Title)
                .Select(p => new UserSufficientType
                {
                    Title = p.Key,
                    Point = p.Sum(o => o.Point),
                    Co2Saving = p.Sum(c => c.Co2Saving)
                });

            Task<IEnumerable<UserSufficientType>> result = Task.FromResult(userSufficientTypes.AsEnumerable());

            Logger.RetrievePointsPerSufficientTypesSuccessful(query.UserId);

            return result;
        }
    }
}