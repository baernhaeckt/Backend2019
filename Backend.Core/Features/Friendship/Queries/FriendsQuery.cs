using System;
using System.Collections.Generic;
using Silverback.Messaging.Messages;

namespace Backend.Core.Features.Friendship.Queries
{
    public class FriendsQuery : IQuery<IEnumerable<FriendsQueryResult>>
    {
        public FriendsQuery(Guid id) => Id = id;

        public Guid Id { get; }
    }
}