using AspNetCore.MongoDB;
using Backend.Database;
using Backend.Models;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;

namespace Backend.Services
{
    public class PointService : PersonalizedService
    {
        public PointService(IMongoOperation<User> userRepository, IPrincipal principal)
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
                MetaData = pointReward.MetaData.Select(m => new Database.MetaData { Key = m.Key, Value = m.Value })
            });
            user.PointActions = pointActions;
            userRepository.SaveAsync(user);
        }
    }
}
