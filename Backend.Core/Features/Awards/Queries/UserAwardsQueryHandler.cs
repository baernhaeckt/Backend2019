using System.Threading.Tasks;
using Backend.Core.Entities;
using Backend.Core.Framework;
using Backend.Infrastructure.Abstraction.Persistence;
using Microsoft.Extensions.Logging;

namespace Backend.Core.Features.Awards.Queries
{
    internal class UserAwardsQueryHandler : QueryHandler<UserAwardsQueryResult, UserAwardsQuery>
    {
        public UserAwardsQueryHandler(IReader reader, ILogger<UserAwardsQueryHandler> logger)
            : base(reader, logger)
        {
        }

        public override async Task<UserAwardsQueryResult> ExecuteAsync(UserAwardsQuery query)
        {
            Logger.RetrieveUserAwards(query.Id);

            UserAwardsQueryResult result = await Reader.GetByIdOrThrowAsync<User, UserAwardsQueryResult>(query.Id, u => new UserAwardsQueryResult(u.Awards));

            Logger.RetrieveUserAwardsSuccessful(query.Id);

            return result;
        }
    }
}