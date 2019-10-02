using System.Threading.Tasks;

namespace Backend.Infrastructure.Geolocation
{
    internal class FakeGeolocationService : IGeolocationService
    {
        public Task<ReverseGeolocationResult> Reverse(string postalCode, string city, string street)
        {
            return Task.FromResult(new ReverseGeolocationResult(0.8, 47.11));
        }
    }
}