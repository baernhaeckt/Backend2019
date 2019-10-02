namespace Backend.Infrastructure.Geolocation
{
    public class ReverseGeolocationResult
    {
        public ReverseGeolocationResult(double longitude, double latitude)
        {
            Longitude = longitude;
            Latitude = latitude;
        }

        public double Latitude { get; }

        public double Longitude { get; }
    }
}