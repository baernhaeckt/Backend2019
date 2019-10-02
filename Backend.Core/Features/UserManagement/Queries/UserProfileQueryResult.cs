namespace Backend.Core.Features.UserManagement.Queries
{
    public class UserProfileQueryResult
    {
        public string DisplayName { get; set; }

        public int Points { get; set; }

        public string Email { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }
    }
}