using AspNetCore.MongoDB;
using Backend.Core.Security.Extensions;
using Backend.Database;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Backend.Core.Services
{
    public class SufficientTypeService : PersonalizedService
    {
        private readonly IMongoOperation<SufficientType> _sufficientTypeRepository;

        public SufficientTypeService(IMongoOperation<SufficientType> sufficientTypeRepository, IMongoOperation<User> userRepository, ClaimsPrincipal principal)
            : base(userRepository, principal)
        {
            _sufficientTypeRepository = sufficientTypeRepository;
        }

        public async Task<IEnumerable<SufficientType>> GetSufficientTypes()
        {
            return await _sufficientTypeRepository.GetAllAsync();
        }

        public async Task<IEnumerable<UserSufficientType>> GetSufficientTypesFromUser()
        {
            User user = await UserRepository.GetByIdAsync(Principal.Id());
            return user.PointActions.GroupBy(p => p.SufficientType.Title).Select(p => new UserSufficientType
            {
                Title = p.Key,
                Point = p.Sum(o => o.Point),
                Co2Saving = p.Sum(c => c.Co2Saving)
            });
        }
    }
}