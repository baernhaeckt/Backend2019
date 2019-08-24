using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNetCore.MongoDB;
using Backend.Core.Newsfeed;
using Backend.Core.Security.Extensions;
using Backend.Database;
using Backend.Models;

namespace Backend.Core.Services
{
    public class UserService : PersonalizedService
    {
        private readonly IEventStream _eventStream;

        public UserService(IMongoOperation<User> userRepository, ClaimsPrincipal principal, IEventStream eventStream)
            : base(userRepository, principal)
        {
            _eventStream = eventStream;
        }



        public async Task Update(Models.UserUpdateRequest updateUserRequest)
        {
            var user = CurrentUser;
            user.DisplayName = updateUserRequest.DisplayName;
            await UserRepository.UpdateAsync(user.Id, user);
        }

        public async Task AddPoints(Token token)
        {
            User user = CurrentUser;
            user.PointActions.Add(new PointAction
            {
                Point = token.Points,
                Action = token.Text,
                Co2Saving = token.Co2Saving,
                SponsorRef = token.Partner,
                SufficientType = new UserSufficientType
                {
                    Title = token.SufficientType.Title
                }
            });

            user.Points += token.Points;
            user.Co2Saving += token.Co2Saving;

            await _eventStream.PublishAsync(new PointsReceivedEvent(user));
            await _eventStream.PublishAsync(new FriendPointsReceivedEvent(user));

            await UserRepository.UpdateAsync(CurrentUser.Id, user);
        }

        public async Task AddPoints(PointAwarding pointAwarding)
        {
            User user = CurrentUser;
            user.PointActions.Add(new PointAction
            {
                Point = pointAwarding.Points,
                Action = pointAwarding.Text,
                Co2Saving = pointAwarding.Co2Saving,
                SponsorRef = pointAwarding.Source.ToString()
            });

            user.Points += pointAwarding.Points;
            user.Co2Saving += pointAwarding.Co2Saving;

            await UserRepository.UpdateAsync(CurrentUser.Id, user);
        }

        public async Task<IEnumerable<PointAction>> PointHistory(string userId)
        {
            var user = await UserRepository.GetByIdAsync(userId);
            return user.PointActions;
        }
    }
}
