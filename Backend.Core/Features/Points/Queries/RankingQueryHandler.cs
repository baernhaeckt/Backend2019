using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Backend.Core.Entities;
using Backend.Core.Framework;
using Backend.Core.Framework.Cqrs;
using Backend.Infrastructure.Abstraction.Persistence;
using Backend.Infrastructure.Abstraction.Security;
using Microsoft.Extensions.Logging;

namespace Backend.Core.Features.Points.Queries
{
    internal class RankingQueryHandler : QueryHandler<RankingSummaryQueryResult, RankingSummaryQuery>
    {
        private const int MaxResults = 100;

        public RankingQueryHandler(IReader reader, ILogger<RankingQueryHandler> logger)
            : base(reader, logger)
        {
        }

        [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Framework demand.")]
        public Task<IEnumerable<RankingQueryResult>> ExecuteAsync(RankingQuery query)
        {
            Logger.RetrieveRanking();

            Task<IEnumerable<RankingQueryResult>> result = Task.FromResult(Ranking().AsEnumerable());

            Logger.RetrieveRankingSuccessful();

            return result;
        }

        public Task<IEnumerable<RankingQueryResult>> ExecuteAsync(RankingByZipQuery query)
        {
            Logger.RetrieveRankingForZip(query.Zip);

            if (int.TryParse(query.Zip, out int parsedZip))
            {
                if (parsedZip >= 1000 && parsedZip < 9999)
                {
                    return Task.FromResult(Ranking(u => u.Location.PostalCode == query.Zip).AsEnumerable());
                }
            }

            Task<IEnumerable<RankingQueryResult>> result = Task.FromResult(Enumerable.Empty<RankingQueryResult>());

            Logger.RetrieveRankingForZipSuccessful(query.Zip);

            return result;
        }

        public Task<IEnumerable<RankingQueryResult>> ExecuteAsync(RankingForUserFriendsQuery query)
        {
            Logger.RetrieveRankingForUserFriends(query.Id);

            Task<IEnumerable<RankingQueryResult>> result = Task.FromResult(Ranking(u => u.Friends.Contains(query.Id)).AsEnumerable());

            Logger.RetrieveRankingForUserFriendsSuccessful(query.Id);

            return result;
        }

        public override async Task<RankingSummaryQueryResult> ExecuteAsync(RankingSummaryQuery query)
        {
            Logger.RetrieveRankingSummary(query.Id);

            RankingSummaryUserProjection userInformation = await Reader.GetByIdOrThrowAsync<User, RankingSummaryUserProjection>(query.Id, u => new RankingSummaryUserProjection(u.Points, u.Location.PostalCode));
            long globalRank = await Reader.CountAsync<User>(u => u.Points > userInformation.Points);
            long localRank = await Reader.CountAsync<User>(u => u.Location.PostalCode == userInformation.Zip && u.Points > userInformation.Points);
            long friendRank = await Reader.CountAsync<User>(u => u.Friends.Contains(query.Id) && u.Points > userInformation.Points);
            var result = new RankingSummaryQueryResult(localRank + 1, globalRank + 1, friendRank + 1);

            Logger.RetrieveRankingSummarySuccessful(query.Id);

            return result;
        }

        private IQueryable<RankingQueryResult> Ranking(Expression<Func<User, bool>>? filter = null)
        {
            IQueryable<User> queryable = Reader.GetQueryable<User>();
            if (filter != null)
            {
                queryable = queryable.Where(filter);
            }

            queryable = queryable.Where(u => !u.Roles.Contains(Roles.Administrator));

            return queryable
                .Select(u => new RankingQueryResult(u.Id, u.DisplayName, u.Points))
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