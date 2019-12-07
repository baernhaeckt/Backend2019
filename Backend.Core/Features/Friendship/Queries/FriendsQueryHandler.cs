using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Core.Entities;
using Backend.Core.Framework;
using Backend.Infrastructure.Abstraction.Persistence;
using Microsoft.Extensions.Logging;

namespace Backend.Core.Features.Friendship.Queries
{
    internal class FriendsQueryHandler : QueryHandler<IEnumerable<FriendsQueryResult>, FriendsQuery>
    {
        public FriendsQueryHandler(IReader reader, ILogger<FriendsQueryHandler> logger)
            : base(reader, logger)
        {
        }

        public override async Task<IEnumerable<FriendsQueryResult>> ExecuteAsync(FriendsQuery query)
        {
            Logger.RetrieveFriendsOfUser(query.Id);

            IEnumerable<Guid> friends = await Reader.GetByIdOrThrowAsync<User, IEnumerable<Guid>>(query.Id, u => u.Friends);
            if (friends == null || !friends.Any())
            {
                return Enumerable.Empty<FriendsQueryResult>();
            }

            IEnumerable<FriendsQueryResult> result = await Reader.WhereAsync<User, FriendsQueryResult>(u => friends.Contains(u.Id), u => new FriendsQueryResult(
                u.Id,
                u.DisplayName,
                u.Points,
                u.Co2Saving,
                u.Email,
                u.Location.Longitude,
                u.Location.Latitude));

            IEnumerable<FriendsQueryResult> friendsQueryResults = result.ToList();
            Logger.RetrieveFriendsOfUserSuccessful(query.Id, friendsQueryResults.Count());

            return friendsQueryResults;
        }
    }
}