using AspNetCore.MongoDB;
using Backend.Core.Security.Extensions;
using Backend.Database;
using System.Linq;
using System.Security.Claims;

namespace Backend.Core.Services
{
    public abstract class PersonalizedService
    {
        protected ClaimsPrincipal Principal { get; }

        protected IMongoOperation<User> userRepository { get; }

        protected User CurrentUser => userRepository.GetQuerableAsync().Single(u => u.Email == Principal.Email());

        protected PersonalizedService(IMongoOperation<User> userRepository, ClaimsPrincipal principal)
        {
            this.userRepository = userRepository;
            Principal = principal;
        }
    }
}
