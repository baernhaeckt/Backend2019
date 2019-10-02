using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Backend.Core.Entities;
using Backend.Core.Extensions;
using Backend.Infrastructure.Persistence.Abstraction;

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

        public async Task<IEnumerable<SufficientType>> GetSufficientTypes() => await _unitOfWork.GetAllAsync<SufficientType>();

        public async Task<IEnumerable<UserSufficientType>> GetSufficientTypesFromUser()
        {
            User user = await _unitOfWork.GetByIdOrDefaultAsync<User>(_principal.Id());
            return user.PointHistory.GroupBy(p => p.SufficientType.Title).Select(p => new UserSufficientType
            {
                Title = p.Key,
                Point = p.Sum(o => o.Point),
                Co2Saving = p.Sum(c => c.Co2Saving)
            });
        }
    }
}