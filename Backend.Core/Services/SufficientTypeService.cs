using Backend.Core.Security.Extensions;
using Backend.Database;
using Backend.Database.Abstraction;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Backend.Core.Services
{
    public class SufficientTypeService : PersonalizedService
    {
        public SufficientTypeService(IUnitOfWork unitOfWork, ClaimsPrincipal principal)
            : base(unitOfWork, principal)
        {
        }

        public async Task<IEnumerable<SufficientType>> GetSufficientTypes() => await UnitOfWork.GetAllAsync<SufficientType>();

        public async Task<IEnumerable<UserSufficientType>> GetSufficientTypesFromUser()
        {
            User user = await UnitOfWork.GetAsync<User>(Principal.Id());
            return user.PointActions.GroupBy(p => p.SufficientType.Title).Select(p => new UserSufficientType
            {
                Title = p.Key,
                Point = p.Sum(o => o.Point),
                Co2Saving = p.Sum(c => c.Co2Saving)
            });
        }
    }
}