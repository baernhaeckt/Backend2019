using System;
using Microsoft.Extensions.Logging;

namespace Backend.Core.Features.Awards
{
    public static class Logging
    {
        public static void ExecuteUserAwardsQuery(this ILogger logger, Guid userId)
        {
            logger.LogInformation(new EventId(1, typeof(Logging).Namespace), "Execute query to return all user awards. User: {userId}", userId);
        }

        public static void CheckUserForNewAwards(this ILogger logger, Guid userId)
        {
            logger.LogInformation(new EventId(2, typeof(Logging).Namespace), "Check if user has achieved a new award. User: {userId}", userId);
        }

        public static void GrantAward(this ILogger logger, Guid userId, string award)
        {
            logger.LogInformation(new EventId(3, typeof(Logging).Namespace), "Grant award to user. UserId: {userId}, Award: {award}", userId, award);
        }
    }
}