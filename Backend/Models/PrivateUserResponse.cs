using Backend.Models;

namespace Backend.Controllers
{
    public class PrivateUserResponse : UserResponse
    {
        public string Email { get; set; }

        public LocationResponse Location { get; set; }
    }
}