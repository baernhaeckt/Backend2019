using System.Threading.Tasks;

namespace Backend.Infrastructure.Abstraction.Geolocation
{
    public interface IGeocodingService
    {
        Task<LookupResult> LookupAsync(string postalCode, string city, string street);
    }
}