using AspNetCore.MongoDB;
using Backend.Database;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Backend.Core.Services
{
    public class PointService : PersonalizedService
    {
        public PointService(IMongoOperation<User> userRepository, ClaimsPrincipal principal)
            : base(userRepository, principal)
        { }

        public int Points
        {
            get => CurrentUser.PointActions.Sum(a => a.Point);
        }

        public IEnumerable<PointAction> History
        {
            get => CurrentUser.PointActions;
        }

        public void RewardPoints(PointReward pointReward)
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
            UserRepository.SaveAsync(user);
        }
    }
}
