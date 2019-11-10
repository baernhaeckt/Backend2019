using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Backend.Core.Entities;
using Backend.Infrastructure.Abstraction.Persistence;
using Backend.Infrastructure.Abstraction.Security;

namespace Backend.Core.Features.Points.Queries
{
    internal class RankingQueryHandler : ISubscriber
    {
        private const int MaxResults = 100;

        public RankingQueryHandler(IReader reader) => _reader = reader;

        public Task<IEnumerable<RankingQueryResult>> ExecuteAsync(RankingQuery query) => Task.FromResult(Ranking().AsEnumerable());

        public Task<IEnumerable<RankingQueryResult>> ExecuteAsync(RankingByZipQuery query)
        {
            if (int.TryParse(query.Zip, out int result))
            {
                if (result >= 1000 && result < 9999)
                {
                    return Task.FromResult(Ranking(u => u.Location.PostalCode == query.Zip).AsEnumerable());
                }
            }

            return Task.FromResult(Enumerable.Empty<RankingQueryResult>());
        }

        public Task<IEnumerable<RankingQueryResult>> ExecuteAsync(RankingForUserFriendsQuery query) => Task.FromResult(Ranking(u => u.Friends.Contains(query.Id)).AsEnumerable());

        public async Task<RankingSummaryQueryResult> ExecuteAsync(RankingSummaryQuery query)
        {
            RankingSummaryUserProjection userInformation = await _reader.GetByIdOrThrowAsync<User, RankingSummaryUserProjection>(query.Id, u => new RankingSummaryUserProjection(u.PointHistory.Sum(pa => pa.Point), u.Location.PostalCode));
            long globalRank = await _reader.CountAsync<User>(u => u.Points > userInformation.Points);
            long localRank = await _reader.CountAsync<User>(u => u.Location.PostalCode == userInformation.Zip && u.Points > userInformation.Points);
            long friendRank = await _reader.CountAsync<User>(u => u.Friends.Contains(query.Id) && u.Points > userInformation.Points);
            return new RankingSummaryQueryResult(localRank + 1, globalRank + 1, friendRank + 1);
        }

        private IQueryable<RankingQueryResult> Ranking(Expression<Func<User, bool>>? filter = null)
        {
            IQueryable<User> queryable = _reader.GetQueryable<User>();
            if (filter != null)
            {
                queryable = queryable.Where(filter);
            }

            queryable = queryable.Where(u => !u.Roles.Contains(Roles.Administrator));

            return queryable.Select(u => new RankingQueryResult(u.Id, u.DisplayName, u.PointHistory.Sum(pa => pa.Point)))
                .OrderByDescending(u => u.Points)
                .Take(MaxResults);
        }

        private class RankingSummaryUserProjection
        {
            public RankingSummaryUserProjection(int points, string? zip)
            {
                Points = points;
                Zip = zip;
            }

            public int Points { get; }

            public string? Zip { get; }
        }
    }
}