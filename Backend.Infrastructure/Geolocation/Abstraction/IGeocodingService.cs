using System.Threading.Tasks;

namespace Backend.Infrastructure.Geolocation.Abstraction
{
    public interface IGeocodingService
    {
        Task<LookupResult> LookupAsync(string postalCode, string city, string street);
    }
}