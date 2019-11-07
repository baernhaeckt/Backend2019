namespace Backend.Core.Features.UserManagement.Queries
{
    public class UserProfileQueryResult
    {
        public UserProfileQueryResult(string displayName, int points, string email, in double locationLatitude, in double locationLongitude, string locationCity, string locationStreet, string city, string locationPostalCode)
        {
            DisplayName = displayName;
            Points = points;
            Email = email;
            Latitude = locationLatitude;
            Longitude = locationLongitude;
            City = locationCity;
            Street = locationStreet;
            PostalCode = locationPostalCode;
        }

        public string DisplayName { get; }

        public int Points { get; }

        public string Email { get; }

        public double Latitude { get; }

        public double Longitude { get; }

        public string PostalCode { get; }

        public string City { get; }

        public string Street { get; }
    }
}