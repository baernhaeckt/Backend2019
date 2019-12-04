using System;
using Microsoft.Extensions.Logging;

namespace Backend.Core.Features.Partner
{
    public static class Logging
    {
        public static void PartnerSignInInitiated(this ILogger logger, Guid id)
        {
            logger.LogInformation(new EventId(1, typeof(Logging).Namespace), "Process sign in request for {id}.", id);
        }

        public static void PartnerSignInPartnerNotFound(this ILogger logger, Guid id)
        {
            logger.LogInformation(new EventId(2, typeof(Logging).Namespace), "No partner with id {id} found.", id);
        }

        public static void PartnerSignInPasswordMismatch(this ILogger logger, Guid id)
        {
            logger.LogInformation(new EventId(3, typeof(Logging).Namespace), "Password mismatch for partner {id}.", id);
        }

        public static void PartnerSignInSuccessful(this ILogger logger, Guid id, string name)
        {
            logger.LogInformation(new EventId(4, typeof(Logging).Namespace), "Successful sign in from partner {name} ({id}).", id, name);
        }

        public static void InitiateRewardUserToken(this ILogger logger, Guid userId, Guid tokenId)
        {
            logger.LogInformation(new EventId(5, typeof(Logging).Namespace), "Initiate reward user token. UserId: {userId}, TokenId: {tokenId}", userId, tokenId);
        }

        public static void RewardUserTokenTokenNotValid(this ILogger logger, Guid userId, Guid tokenId, bool isSingleUse, bool isAlreadyUsed)
        {
            logger.LogInformation(new EventId(6, typeof(Logging).Namespace), "Successful rewarded user token. UserId: {userId}, TokenId: {tokenId}, IsSingleUse: {isSingleUse}, IsAlreadyUsed: {isAlreadyUsed}", userId, tokenId, isSingleUse, isAlreadyUsed);
        }

        public static void RewardUserTokenSuccessful(this ILogger logger, Guid userId, Guid tokenId)
        {
            logger.LogInformation(new EventId(7, typeof(Logging).Namespace), "Successful rewarded user token. UserId: {userId}, TokenId: {tokenId}", userId, tokenId);
        }

        public static void InitiateCreateNewToken(this ILogger logger, Guid partnerId, string tokenType)
        {
            logger.LogInformation(new EventId(8, typeof(Logging).Namespace), "Initiate token creation. PartnerId: {partnerId}, TokenId: {tokenId}", partnerId, tokenType);
        }

        public static void CreateNewTokenSuccessful(this ILogger logger, Guid partnerId, string tokenType, Guid tokenId)
        {
            logger.LogInformation(new EventId(9, typeof(Logging).Namespace), "Successful created new token. PartnerId: {partnerId}, TokenType: {tokenType}, TokenId: {tokenId}", partnerId, tokenType, tokenId);
        }
    }
}