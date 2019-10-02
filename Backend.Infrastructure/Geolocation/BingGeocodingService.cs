using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Infrastructure.Geolocation.Abstraction;
using Geocoding;
using Geocoding.Microsoft;
using Microsoft.Extensions.Options;

namespace Backend.Infrastructure.Geolocation
{
    internal class BingGeocodingService : IGeocodingService
    {
        private readonly IOptions<GeocodingOptions> _options;

        public BingGeocodingService(IOptions<GeocodingOptions> options)
        {
            _options = options;
        }

        public async Task<LookupResult> LookupAsync(string postalCode, string city, string street)
        {
            IGeocoder geocoder = new BingMapsGeocoder(_options.Value.ApiKey);
            IEnumerable<Address> addresses = await geocoder.GeocodeAsync(street, city, null, postalCode, "Switzerland");
            if (!addresses.Any())
            {
                return new LookupResult();
            }

            return new LookupResult(addresses.First().Coordinates.Longitude, addresses.First().Coordinates.Latitude);
        }
    }
}