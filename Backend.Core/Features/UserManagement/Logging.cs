using System;
using Backend.Core.Features.UserManagement.Commands;
using Microsoft.Extensions.Logging;

namespace Backend.Core.Features.UserManagement
{
    public static class Logging
    {
        public static void ExecuteUserSignIn(this ILogger logger, string email)
        {
            logger.LogInformation(new EventId(1, typeof(Logging).Namespace), "Process sign in request for user. Email: {email}", email);
        }

        public static void UserSignInUserNotFound(this ILogger logger, string email)
        {
            logger.LogInformation(new EventId(2, typeof(Logging).Namespace), "No user found. Email: {email}", email);
        }

        public static void UserSignInPasswordMismatch(this ILogger logger, string email)
        {
            logger.LogInformation(new EventId(3, typeof(Logging).Namespace), "Password mismatch for user. Email: {email}", email);
        }

        public static void ExecuteUserSignInSuccessful(this ILogger logger, string email)
        {
            logger.LogInformation(new EventId(4, typeof(Logging).Namespace), "Executed sign in. Email: {email}", email);
        }

        public static void UserUnableToLookupAddress(this ILogger logger, string city, string street, string postalCode)
        {
            logger.LogWarning(new EventId(5, typeof(Logging).Namespace), "Unable to get coordinates for location. City: {city}, Street: {street}, PostalCode: {postalCode}", city, street, postalCode);
        }

        public static void ExecuteUserPasswordChange(this ILogger logger, Guid userId)
        {
            logger.LogInformation(new EventId(6, typeof(Logging).Namespace), "Execute change user password. UserId: {userId}", userId);
        }

        public static void UserPasswordChangeOldPasswordNotMatched(this ILogger logger, Guid userId)
        {
            logger.LogInformation(new EventId(7, typeof(Logging).Namespace), "Password change not successful because the old password didn't match. UserId: {userId}", userId);
        }

        public static void ExecuteUserPasswordChangeChangeSuccessful(this ILogger logger, Guid userId)
        {
            logger.LogInformation(new EventId(8, typeof(Logging).Namespace), "Executed changed user password. UserId: {userId}", userId);
        }

        public static void ExecuteUserRegistration(this ILogger logger, string userEmail)
        {
            logger.LogInformation(new EventId(9, typeof(Logging).Namespace), "Execute register new user. Email: {userEmail}", userEmail);
        }

        public static void ExecuteUserRegistrationSuccessful(this ILogger logger, Guid userId, string userEmail)
        {
            logger.LogInformation(new EventId(10, typeof(Logging).Namespace), "Executed register new user. UserId: {userId}, UserEmail: {userEmail}", userId, userEmail);
        }

        public static void ExecuteUserProfileUpdate(this ILogger logger, UpdateProfileCommand command)
        {
            logger.LogInformation(new EventId(11, typeof(Logging).Namespace), "Execute update profile. UpdateCommand: {@command}.", command);
        }

        public static void ExecuteUserProfileUpdateSuccessful(this ILogger logger, Guid userId)
        {
            logger.LogInformation(new EventId(12, typeof(Logging).Namespace), "Executed update user profile. UserId: {userId}.", userId);
        }

        public static void HandleUserRegisteredEvent(this ILogger logger, Guid userId)
        {
            logger.LogInformation(new EventId(13, typeof(Logging).Namespace), "Handle UserRegisteredEvent: Send e-mail. UserId: {userId}", userId);
        }

        public static void HandleUserRegisteredEventSuccessful(this ILogger logger, Guid userId)
        {
            logger.LogInformation(new EventId(14, typeof(Logging).Namespace), "Handled UserRegisteredEvent. UserId: {userId}.", userId);
        }

        public static void RetrieveUserProfile(this ILogger logger, Guid userId)
        {
            logger.LogInformation(new EventId(15, typeof(Logging).Namespace), "Retrieve user profile. UserId: {userId}.", userId);
        }

        public static void RetrieveUserProfileSuccessful(this ILogger logger, Guid userId)
        {
            logger.LogInformation(new EventId(16, typeof(Logging).Namespace), "Retrieved user profile. UserId: {userId}.", userId);
        }

        public static void RetrieveSecurityTokenForUser(this ILogger logger, string email)
        {
            logger.LogInformation(new EventId(17, typeof(Logging).Namespace), "Retrieve security token. Email: {email}.", email);
        }

        public static void RetrieveSecurityTokenForUserSuccessful(this ILogger logger, string email, Guid id)
        {
            logger.LogInformation(new EventId(18, typeof(Logging).Namespace), "Retrieved security token. Email: {email}. UserId: {id}", email, id);
        }

        public static void RetrieveEmailAlreadyRegistered(this ILogger logger, string email)
        {
            logger.LogInformation(new EventId(19, typeof(Logging).Namespace), "Retrieve if there is already a user with this email. Email: {email}.", email);
        }

        public static void RetrieveEmailAlreadyRegisteredSuccessful(this ILogger logger, string email, bool alreadyRegistered)
        {
            logger.LogInformation(new EventId(20, typeof(Logging).Namespace), "Retrieved if there is already a user with this email. Email: {email}, AlreadyRegistered: {alreadyRegistered}", email, alreadyRegistered);
        }
    }
}