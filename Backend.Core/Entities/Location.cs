using Backend.Infrastructure.Persistence.Abstraction;

namespace Backend.Core.Entities
{
    public class Location
    {
        public double Longitude { get; set; }

        public double Latitude { get; set; }

        public string Zip { get; set; } = string.Empty;

        public string City { get; set; } = string.Empty;
    }
}