using System.Security.Claims;
using AspNetCore.MongoDB;
using Backend.Database;

namespace Backend.Core.Services
{
    public class UserService : PersonalizedService
    {
        public UserService(IMongoOperation<User> userRepository, ClaimsPrincipal principal) 
            : base(userRepository, principal)
        {
        }

        public new User CurrentUser => base.CurrentUser;

        public void Update(Backend.Models.UserUpdateRequest updateUserRequest)
        {

        }
    }
}
