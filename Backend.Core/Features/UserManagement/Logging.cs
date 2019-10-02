using Microsoft.Extensions.Logging;

namespace Backend.Core.Features.UserManagement
{
    public static class Logging
    {
        public static void UnableToLookupAddress(this ILogger logger, string city, string street, string postalCode)
        {
            logger.LogWarning(new EventId(1), nameof(UserManagement), "Unable to get coordinates for location. {city}, {street}, {postalCode}", city, street, postalCode);
        }
    }
}
