namespace Backend.Web.Models
{
    public class PrivateUserResponse : UserResponse
    {
        public string Email { get; set; }

        public LocationResponse Location { get; set; }
    }
}