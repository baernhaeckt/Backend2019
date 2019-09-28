using Backend.Core.Security.Extensions;
using Backend.Database;
using Backend.Database.Abstraction;
using System.Security.Claims;

namespace Backend.Core.Services
{
    public abstract class PersonalizedService
    {
        protected IUnitOfWork UnitOfWork { get; }

        protected ClaimsPrincipal Principal { get; }

        // TODO: Refactor to method.
        public User CurrentUser => UnitOfWork.SingleOrDefaultAsync<User>(u => u.Email == Principal.Email()).Result;

        protected PersonalizedService(IUnitOfWork unitOfWork, ClaimsPrincipal principal)
        {
            UnitOfWork = unitOfWork;
            Principal = principal;
        }
    }
}
