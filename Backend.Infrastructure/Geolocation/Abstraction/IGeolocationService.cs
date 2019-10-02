using System.Threading.Tasks;

namespace Backend.Infrastructure.Geolocation
{
    public interface IGeolocationService
    {
        Task<ReverseGeolocationResult> Reverse(string postalCode, string city, string street);
    }
}