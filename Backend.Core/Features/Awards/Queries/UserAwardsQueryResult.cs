using System.Collections.Generic;
using Backend.Core.Entities.Awards;

namespace Backend.Core.Features.Awards.Queries
{
    public class UserAwardsQueryResult
    {
        public UserAwardsQueryResult(IEnumerable<Award> awards) => Awards = awards;

        public IEnumerable<Award> Awards { get; }
    }
}