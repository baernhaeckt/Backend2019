using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Backend.Infrastructure.Abstraction.Geolocation;
using Geocoding;
using Geocoding.Microsoft;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Backend.Infrastructure.Geolocation
{
    [ExcludeFromCodeCoverage]
    internal class BingGeocodingService : IGeocodingService
    {
        private readonly ILogger<BingGeocodingService> _logger;

        private readonly IOptions<GeocodingOptions> _options;

        public BingGeocodingService(IOptions<GeocodingOptions> options, ILogger<BingGeocodingService> logger)
        {
            _options = options;
            _logger = logger;
        }

        public async Task<LookupResult> LookupAsync(string postalCode, string city, string street)
        {
            _logger.GeocodingStartLookup(postalCode, city, street);

            IGeocoder geocoder = new BingMapsGeocoder(_options.Value.ApiKey);
            IList<Address> addresses = (await geocoder.GeocodeAsync(street, city, null, postalCode, "Switzerland")).ToList();
            if (!addresses.Any())
            {
                _logger.GeocodingLookupNoResultsFound(postalCode, city, street);
                return new LookupResult();
            }

            if (addresses.Count > 1)
            {
                _logger.GeocodingLookupMultipleAddressesFound(postalCode, city, street, string.Join(';', addresses.Select(a => a.FormattedAddress)));
            }

            return new LookupResult(addresses.First().Coordinates.Longitude, addresses.First().Coordinates.Latitude);
        }
    }
}