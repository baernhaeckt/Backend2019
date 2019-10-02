using System.Threading.Tasks;
using Backend.Infrastructure.Geolocation.Abstraction;

namespace Backend.Infrastructure.Geolocation
{
    internal class FakeGeocodingService : IGeocodingService
    {
        public Task<LookupResult> LookupAsync(string postalCode, string city, string street) => Task.FromResult(new LookupResult(7.443788, 46.944699));
    }
}