namespace Backend.Core.Features.UserManagement.Models
{
    public class UserLoginRequest
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }
}