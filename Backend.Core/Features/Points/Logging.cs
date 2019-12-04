using System;
using Microsoft.Extensions.Logging;

namespace Backend.Core.Features.Points
{
    public static class Logging
    {
        public static void PartnerInitiateGrantPointsForToken(this ILogger logger, Guid userId, Guid tokenId)
        {
            logger.LogInformation(new EventId(1, typeof(Logging).Namespace), "Grant points for a partner token. User: {userId} TokenId: {tokenId}", userId, tokenId);
        }

        public static void PartnerGrantPointsForTokenUpdateUser(this ILogger logger, object updateObject)
        {
            logger.LogInformation(new EventId(2, typeof(Logging).Namespace), "Grant points for a partner token. About to update User. Details: {@updateObject}", updateObject);
        }

        public static void PartnerSuccessfulGrantPointsForToken(this ILogger logger, Guid userId, Guid tokenId)
        {
            logger.LogInformation(new EventId(3, typeof(Logging).Namespace), "Successful granted points for a partner token. User: {userId} TokenId: {tokenId}", userId, tokenId);
        }

        public static void PartnerInitiateGrantPointsForCorrectQuizAnswer(this ILogger logger, Guid userId, int points)
        {
            logger.LogInformation(new EventId(4, typeof(Logging).Namespace), "Grant points for correct quiz answer. User: {userId} Points: {points}", userId, points);
        }

        public static void PartnerGrantPointsForCorrectQuizAnswerUpdateUser(this ILogger logger, object updateObject)
        {
            logger.LogInformation(new EventId(5, typeof(Logging).Namespace), "Grant points for a correct quiz answer. About to update User. Details: {@updateObject}", updateObject);
        }

        public static void PartnerSuccessfulGrantPointsForCorrectQuizAnswer(this ILogger logger, Guid userId, int points)
        {
            logger.LogInformation(new EventId(6, typeof(Logging).Namespace), "Successful granted points for a correct quiz answer. User: {userId} TokenId: {tokenId}", userId, points);
        }
    }
}