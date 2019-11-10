using System;
using System.Collections.Generic;
using Silverback.Messaging.Messages;

namespace Backend.Core.Features.Points.Queries
{
    public class RankingForUserFriendsQuery : IQuery<IEnumerable<RankingQueryResult>>
    {
        public RankingForUserFriendsQuery(Guid id) => Id = id;

        public Guid Id { get; }
    }
}