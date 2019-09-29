using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Backend.Core.Extensions;
using Backend.Database.Abstraction;
using Backend.Database.Entities;

namespace Backend.Core.Features.Baseline
{
    public class SufficientTypeService
    {
        private readonly ClaimsPrincipal _principal;

        private readonly IUnitOfWork _unitOfWork;

        public SufficientTypeService(IUnitOfWork unitOfWork, ClaimsPrincipal principal)
        {
            _unitOfWork = unitOfWork;
            _principal = principal;
        }

        public async Task<IEnumerable<SufficientType>> GetSufficientTypes()
        {
            return await _unitOfWork.GetAllAsync<SufficientType>();
        }

        public async Task<IEnumerable<UserSufficientType>> GetSufficientTypesFromUser()
        {
            User user = await _unitOfWork.GetAsync<User>(_principal.Id());
            return user.PointActions.GroupBy(p => p.SufficientType.Title).Select(p => new UserSufficientType
            {
                Title = p.Key,
                Point = p.Sum(o => o.Point),
                Co2Saving = p.Sum(c => c.Co2Saving)
            });
        }
    }
}