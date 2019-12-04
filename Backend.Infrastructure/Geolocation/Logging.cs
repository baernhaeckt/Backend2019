using Microsoft.Extensions.Logging;

namespace Backend.Infrastructure.Geolocation
{
    public static class Logging
    {
        public static void GeocodingStartLookup(this ILogger logger, string postalCode, string city, string street)
        {
            logger.LogInformation(new EventId(1, typeof(Logging).Namespace), "Start looking up address for: PostalCode: {postalCode}, City: {city}, Street: {street}", postalCode, city, street);
        }

        public static void GeocodingLookupNoResultsFound(this ILogger logger, string postalCode, string city, string street)
        {
            logger.LogInformation(new EventId(2, typeof(Logging).Namespace), "Lookup didn't return any addresses for: PostalCode: {postalCode}, City: {city}, Street: {street}", postalCode, city, street);
        }

        public static void GeocodingLookupMultipleAddressesFound(this ILogger logger, string postalCode, string city, string street, string addresses)
        {
            logger.LogInformation(new EventId(3, typeof(Logging).Namespace), "Lookup return multiple addresses for: PostalCode: {postalCode}, City: {city}, Street: {street}. Addresses: {addresses}", postalCode, city, street, addresses);
        }
    }
}