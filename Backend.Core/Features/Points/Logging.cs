using System;
using Microsoft.Extensions.Logging;

namespace Backend.Core.Features.Points
{
    public static class Logging
    {
        public static void HandlePartnerTokenRegisteredEvent(this ILogger logger, Guid userId, Guid tokenId)
        {
            logger.LogInformation(new EventId(1, typeof(Logging).Namespace), "Handle PartnerTokenRegisteredEvent: Grant points for a partner token. User: {userId} TokenId: {tokenId}", userId, tokenId);
        }

        public static void PartnerGrantPointsForTokenUpdateUser(this ILogger logger, object updateObject)
        {
            logger.LogInformation(new EventId(2, typeof(Logging).Namespace), "Grant points for a partner token. About to update User. Details: {@updateObject}", updateObject);
        }

        public static void HandlePartnerTokenRegisteredEventSuccessful(this ILogger logger, Guid userId, Guid tokenId)
        {
            logger.LogInformation(new EventId(3, typeof(Logging).Namespace), "Handled PartnerTokenRegisteredEvent. User: {userId}, TokenId: {tokenId}", userId, tokenId);
        }

        public static void HandleQuizQuestionCorrectAnsweredEvent(this ILogger logger, Guid userId, int points)
        {
            logger.LogInformation(new EventId(4, typeof(Logging).Namespace), "Handle QuizQuestionCorrectAnsweredEvent: Grant points for correct quiz answer. User: {userId} Points: {points}", userId, points);
        }

        public static void PartnerGrantPointsForCorrectQuizAnswerUpdateUser(this ILogger logger, object updateObject)
        {
            logger.LogInformation(new EventId(5, typeof(Logging).Namespace), "Grant points for a correct quiz answer. About to update User. Details: {@updateObject}", updateObject);
        }

        public static void HandleQuizQuestionCorrectAnsweredEventSuccessful(this ILogger logger, Guid userId, int points)
        {
            logger.LogInformation(new EventId(6, typeof(Logging).Namespace), "Handled QuizQuestionCorrectAnsweredEvent. User: {userId}, TokenId: {tokenId}", userId, points);
        }

        public static void RetrieveRankingForUserFriends(this ILogger logger, Guid userId)
        {
            logger.LogInformation(new EventId(7, typeof(Logging).Namespace), "Retrieve ranking for friends. User: {userId}", userId);
        }

        public static void RetrieveRankingForUserFriendsSuccessful(this ILogger logger, Guid userId)
        {
            logger.LogInformation(new EventId(8, typeof(Logging).Namespace), "Retrieved ranking for friends. User: {userId}", userId);
        }

        public static void RetrieveRankingSummary(this ILogger logger, Guid userId)
        {
            logger.LogInformation(new EventId(9, typeof(Logging).Namespace), "Retrieve ranking summary. User: {userId}", userId);
        }

        public static void RetrieveRankingSummarySuccessful(this ILogger logger, Guid userId)
        {
            logger.LogInformation(new EventId(10, typeof(Logging).Namespace), "Retrieved ranking summary. User: {userId}", userId);
        }

        public static void RetrieveRankingForZip(this ILogger logger, string zip)
        {
            logger.LogInformation(new EventId(11, typeof(Logging).Namespace), "Retrieve ranking for zip. Zip: {zip}", zip);
        }

        public static void RetrieveRankingForZipSuccessful(this ILogger logger, string zip)
        {
            logger.LogInformation(new EventId(12, typeof(Logging).Namespace), "Retrieved ranking for zip. Zip: {zip}", zip);
        }

        public static void RetrieveRanking(this ILogger logger)
        {
            logger.LogInformation(new EventId(13, typeof(Logging).Namespace), "Retrieve ranking");
        }

        public static void RetrieveRankingSuccessful(this ILogger logger)
        {
            logger.LogInformation(new EventId(14, typeof(Logging).Namespace), "Retrieved ranking.");
        }

        public static void RetrievePointHistoryForUser(this ILogger logger, Guid userId)
        {
            logger.LogInformation(new EventId(15, typeof(Logging).Namespace), "Retrieve point history for user. UserId: {userId}", userId);
        }

        public static void RetrievePointHistoryForUserSuccessful(this ILogger logger, Guid userId)
        {
            logger.LogInformation(new EventId(16, typeof(Logging).Namespace), "Retrieved point history for user. UserId: {userId}", userId);
        }
    }
}