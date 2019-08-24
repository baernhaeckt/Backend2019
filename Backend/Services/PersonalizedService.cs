using AspNetCore.MongoDB;
using Backend.Database;
using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace Backend.Services
{
    public abstract class PersonalizedService
    {
        protected String currentUserId => Principal.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;

        protected ClaimsPrincipal Principal { get; }

        protected IMongoOperation<User> userRepository { get; }

        protected User CurrentUser 
            => userRepository.GetQuerableAsync().Single(u => u.Email == currentUserId);

        protected PersonalizedService(IMongoOperation<User> userRepository, ClaimsPrincipal principal)
        {
            this.userRepository = userRepository;
            Principal = principal;
        }
    }
}
