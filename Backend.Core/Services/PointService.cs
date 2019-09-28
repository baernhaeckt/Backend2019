using Backend.Database;
using Backend.Database.Abstraction;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Backend.Core.Services
{
    public class PointService : PersonalizedService
    {
        public PointService(IUnitOfWork unitOfWork, ClaimsPrincipal principal)
            : base(unitOfWork, principal)
        { }

        public int Points => CurrentUser.PointActions.Sum(a => a.Point);

        public IEnumerable<PointAction> History => CurrentUser.PointActions;

        public async Task RewardPointsAsync(PointReward pointReward)
        {
            User user = CurrentUser;
            var pointActions = user.PointActions.ToList();
            pointActions.Add(new PointAction
            {
                Action = pointReward.Text,
                Point = pointReward.Value,
                MetaData = pointReward.MetaData.Select(m => new MetaData { Key = m.Key, Value = m.Value })
            });
            user.PointActions = pointActions;
            await UnitOfWork.UpdateAsync(user);
        }
    }
}
