namespace Backend.Infrastructure.Abstraction.Geolocation
{
    public class LookupResult
    {
        public LookupResult(double longitude, double latitude)
        {
            Failed = false;
            Longitude = longitude;
            Latitude = latitude;
        }

        public LookupResult() => Failed = true;

        public bool Failed { get; }

        public double Latitude { get; }

        public double Longitude { get; }
    }
}