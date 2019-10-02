using System;
using Silverback.Messaging.Messages;

namespace Backend.Core.Features.Awards.Queries
{
    public class UserAwardsQuery : IQuery<UserAwardsQueryResult>
    {
        public UserAwardsQuery(Guid id) => Id = id;

        public Guid Id { get; }
    }
}