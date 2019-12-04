using System;
using Backend.Core.Features.UserManagement.Commands;
using Microsoft.Extensions.Logging;

namespace Backend.Core.Features.UserManagement
{
    public static class Logging
    {
        public static void UserSignInInitiated(this ILogger logger, string email)
        {
            logger.LogInformation(new EventId(1, typeof(Logging).Namespace), "Process sign in request for {email}.", email);
        }

        public static void UserSignInUserNotFound(this ILogger logger, string email)
        {
            logger.LogInformation(new EventId(2, typeof(Logging).Namespace), "No partner with id {email} found.", email);
        }

        public static void UserSignInPasswordMismatch(this ILogger logger, string email)
        {
            logger.LogInformation(new EventId(3, typeof(Logging).Namespace), "Password mismatch for user {email}.", email);
        }

        public static void UserSignInSuccessful(this ILogger logger, string email)
        {
            logger.LogInformation(new EventId(4, typeof(Logging).Namespace), "Successful sign in from partner {email}.", email);
        }

        public static void UserUnableToLookupAddress(this ILogger logger, string city, string street, string postalCode)
        {
            logger.LogWarning(new EventId(5, typeof(Logging).Namespace), "Unable to get coordinates for location. {city}, {street}, {postalCode}", city, street, postalCode);
        }

        public static void UserPasswordChangeInitiated(this ILogger logger, Guid userId)
        {
            logger.LogInformation(new EventId(6, typeof(Logging).Namespace), "Change user password. UserId: {userId}", userId);
        }

        public static void UserPasswordChangeOldPasswordNotMatched(this ILogger logger, Guid userId)
        {
            logger.LogInformation(new EventId(8, typeof(Logging).Namespace), "Password change not successful because the old password didn't match. UserId: {userId}", userId);
        }

        public static void UserPasswordChangeChangeSuccessful(this ILogger logger, Guid userId)
        {
            logger.LogInformation(new EventId(8, typeof(Logging).Namespace), "Successful changed user password. UserId: {userId}", userId);
        }

        public static void UserInitiateRegistration(this ILogger logger, string userEmail)
        {
            logger.LogInformation(new EventId(9, typeof(Logging).Namespace), "Register new user. Email: {userEmail}", userEmail);
        }

        public static void UserRegistrationSuccessful(this ILogger logger, Guid userId, string userEmail)
        {
            logger.LogInformation(new EventId(10, typeof(Logging).Namespace), "Successful registered new user. UserId: {userId}, UserEmail: {userEmail}", userId, userEmail);
        }

        public static void UserInitiateProfileUpdate(this ILogger logger, UpdateProfileCommand command)
        {
            logger.LogInformation(new EventId(11, typeof(Logging).Namespace), "Update profile of user. UpdateCommand: {@command}.", command);
        }

        public static void UserProfileUpdateSuccessful(this ILogger logger, Guid userId)
        {
            logger.LogInformation(new EventId(12, typeof(Logging).Namespace), "Successful updated user profile. UserId: {userId}.", userId);
        }

        public static void UserHandleUserRegisteredEvent(this ILogger logger, Guid userId)
        {
            logger.LogInformation(new EventId(13, typeof(Logging).Namespace), "Handle user registered event. UserId: {userId}", userId);
        }

        public static void UserHandleUserRegisteredEventSuccessful(this ILogger logger, Guid userId)
        {
            logger.LogInformation(new EventId(14, typeof(Logging).Namespace), "Successful handled user registered event. UserId: {userId}.", userId);
        }

        public static void ExecuteUserProfileQuery(this ILogger logger, Guid userId)
        {
            logger.LogInformation(new EventId(15, typeof(Logging).Namespace), "Retrieve user profile. UserId: {userId}.", userId);
        }

        public static void ExecuteSecurityTokenForUserQuery(this ILogger logger, string email)
        {
            logger.LogInformation(new EventId(16, typeof(Logging).Namespace), "Generate SecurityToken. Email: {email}.", email);
        }

        public static void ExecuteEmailRegisteredQueryHandler(this ILogger logger, string email)
        {
            logger.LogInformation(new EventId(17, typeof(Logging).Namespace), "Lookup if there is already a user with this email. Email: {email}.", email);
        }

        public static void NoUserWithThisEmailFound(this ILogger logger, string email)
        {
            logger.LogInformation(new EventId(18, typeof(Logging).Namespace), "No user with this email exists. Email: {email}.", email);
        }

        public static void FoundAlreadyRegisteredUsers(this ILogger logger, string email, long count)
        {
            logger.LogInformation(new EventId(19, typeof(Logging).Namespace), "There are already users with this email. Email: {email} Count: {count}.", email, count);
        }
    }
}
