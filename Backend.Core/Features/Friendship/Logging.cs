using System;
using Microsoft.Extensions.Logging;

namespace Backend.Core.Features.Friendship
{
    public static class Logging
    {
        public static void ExecuteAddFriend(this ILogger logger, Guid userId, string friendEmail)
        {
            logger.LogInformation(new EventId(1, typeof(Logging).Namespace), "Execute add friend. UserId: {userId}, FriendEmail: {friendEmail}", userId, friendEmail);
        }

        public static void ExecuteAddFriendSuccessful(this ILogger logger, Guid userId, string friendEmail, Guid friendId)
        {
            logger.LogInformation(new EventId(2, typeof(Logging).Namespace), "Execute add friend successful. UserId: {userId}, FriendEmail: {friendEmail}, FriendId: {friendId}", userId, friendEmail, friendId);
        }

        public static void ExecuteRemoveFriend(this ILogger logger, Guid userId, Guid friendId)
        {
            logger.LogInformation(new EventId(3, typeof(Logging).Namespace), "Execute remove friend. UserId: {userId}, FriendId: {friendId}", userId, friendId);
        }

        public static void RemoveFriendSuccessful(this ILogger logger, Guid userId, Guid friendId)
        {
            logger.LogInformation(new EventId(4, typeof(Logging).Namespace), "Removed friend. UserId: {userId}, FriendId: {friendId}", userId, friendId);
        }

        public static void RetrieveFriendsOfUser(this ILogger logger, Guid userId)
        {
            logger.LogInformation(new EventId(5, typeof(Logging).Namespace), "Retrieve friends. UserId: {userId}", userId);
        }

        public static void RetrieveFriendsOfUserSuccessful(this ILogger logger, Guid userId, int friendCount)
        {
            logger.LogInformation(new EventId(6, typeof(Logging).Namespace), "Retrieved friends. UserId: {userId}, FriendCount: {friendCount}", userId, friendCount);
        }
    }
}