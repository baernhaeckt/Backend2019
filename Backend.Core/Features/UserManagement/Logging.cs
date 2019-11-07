using Microsoft.Extensions.Logging;

namespace Backend.Core.Features.UserManagement
{
    public static class Logging
    {
        public static void UnableToLookupAddress(this ILogger logger, string city, string street, string postalCode)
        {
            logger.LogWarning(new EventId(5), nameof(Logging), "Unable to get coordinates for location. {city}, {street}, {postalCode}", city, street, postalCode);
        }

        public static void UserSignInInitiated(this ILogger logger, string email)
        {
            logger.LogInformation(new EventId(1), typeof(Logging).Namespace, "Process sign in request for {email}.", email);
        }

        public static void UserSignInUserNotFound(this ILogger logger, string email)
        {
            logger.LogInformation(new EventId(2), typeof(Logging).Namespace, "No partner with id {email} found.", email);
        }

        public static void UserSignInPasswordMismatch(this ILogger logger, string email)
        {
            logger.LogInformation(new EventId(3), typeof(Logging).Namespace, "Password mismatch for user {email}.", email);
        }

        public static void UserSignInSuccessful(this ILogger logger, string email)
        {
            logger.LogInformation(new EventId(4), typeof(Logging).Namespace, "Successful sign in from partner {email}.");
        }
    }
}
