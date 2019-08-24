using AspNetCore.MongoDB;
using Backend.Database;
using System;
using System.Linq;
using System.Security.Principal;

namespace Backend.Services
{
    public abstract class PersonalizedService
    {
        protected String currentUserId
        {
            get { return Principal.Identity.Name; }
        }
        protected IPrincipal Principal { get; }

        protected IMongoOperation<User> userRepository { get; }

        protected User CurrentUser
        {
            get => userRepository.GetQuerableAsync()
                .Single(u => u.Email.Equals(currentUserId, StringComparison.InvariantCultureIgnoreCase);
        }

        protected PersonalizedService(IMongoOperation<User> userRepository, IPrincipal principal)
        {
            this.userRepository = userRepository;
            Principal = principal;
        }

    }
}
