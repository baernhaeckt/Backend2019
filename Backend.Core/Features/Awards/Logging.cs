using System;
using Microsoft.Extensions.Logging;

namespace Backend.Core.Features.Awards
{
    public static class Logging
    {
        public static void RetrieveUserAwards(this ILogger logger, Guid userId)
        {
            logger.LogInformation(new EventId(1, typeof(Logging).Namespace), "Retrieve all awards of user. User: {userId}", userId);
        }

        public static void RetrieveUserAwardsSuccessful(this ILogger logger, Guid userId)
        {
            logger.LogInformation(new EventId(2, typeof(Logging).Namespace), "Retrieved all awards of user. User: {userId}", userId);
        }

        public static void HandleUserNewPointsEvent(this ILogger logger, Guid userId)
        {
            logger.LogInformation(new EventId(3, typeof(Logging).Namespace), "Handle UserNewPointsEvent: Check if user has achieved a new award. User: {userId}", userId);
        }

        public static void GrantAward(this ILogger logger, Guid userId, string award)
        {
            logger.LogInformation(new EventId(4, typeof(Logging).Namespace), "Grant award to user. UserId: {userId}, Award: {award}", userId, award);
        }

        public static void HandleUserNewPointsEventSuccessful(this ILogger logger, Guid userId, int awardCount)
        {
            logger.LogInformation(new EventId(5, typeof(Logging).Namespace), "Handled UserNewPointsEvent. User: {userId}, AwardCount: {awardCount}", userId, awardCount);
        }
    }
}