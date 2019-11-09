using System.Threading.Tasks;
using Backend.Core.Entities;
using Backend.Infrastructure.Abstraction.Persistence;
using Silverback.Messaging.Subscribers;

namespace Backend.Core.Features.Awards.Queries
{
    internal class UserAwardsQueryQueryHandler : ISubscriber
    {
        private readonly IReader _reader;

        public UserAwardsQueryQueryHandler(IReader reader) => _reader = reader;

        public async Task<UserAwardsQueryResult> ExecuteAsync(UserAwardsQuery query)
        {
            UserAwardsQueryResult result = await _reader.GetByIdOrThrowAsync<User, UserAwardsQueryResult>(query.Id, u => new UserAwardsQueryResult(u.Awards));
            return result;
        }
    }
}