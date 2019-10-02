namespace Backend.Core.Features.UserManagement.Queries
{
    public class UserProfileQueryResult
    {
        public UserProfileQueryResult(string displayName, int points, string email, double latitude, double longitude)
        {
            DisplayName = displayName;
            Points = points;
            Email = email;
            Latitude = latitude;
            Longitude = longitude;
        }

        public string DisplayName { get; }

        public int Points { get; }

        public string Email { get; }

        public double Latitude { get; }

        public double Longitude { get; }
    }
}