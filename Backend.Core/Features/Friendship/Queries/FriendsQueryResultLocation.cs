namespace Backend.Core.Features.Friendship.Queries
{
    public class FriendsQueryResultLocation
    {
        public FriendsQueryResultLocation(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public double Latitude { get; }

        public double Longitude { get; }
    }
}