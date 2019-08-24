using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNetCore.MongoDB;
using Backend.Core.Newsfeed;
using Backend.Database;
using Backend.Models;

namespace Backend.Core.Services
{
    public class UserService : PersonalizedService
    {
        private readonly IEventStream _eventStream;
        private readonly AwardService _awardService;

        public UserService(IMongoOperation<User> userRepository, ClaimsPrincipal principal, IEventStream eventStream, AwardService awardService)
            : base(userRepository, principal)
        {
            _eventStream = eventStream;
            _awardService = awardService;
        }

        public async Task Update(UserUpdateRequest updateUserRequest)
        {
            var user = CurrentUser;
            user.DisplayName = updateUserRequest.DisplayName;
            await UserRepository.UpdateAsync(user.Id, user);
        }

        public async Task AddPoints(Token token)
        {
            CurrentUser.PointActions.Add(new PointAction
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

            await Process(token.Points, token.Co2Saving, CurrentUser);
        }

        public async Task AddPoints(PointAwarding pointAwarding)
        {
            CurrentUser.PointActions.Add(new PointAction
            {
                Point = pointAwarding.Points,
                Action = pointAwarding.Text,
                Co2Saving = pointAwarding.Co2Saving,
                SponsorRef = pointAwarding.Source.ToString()
            });

            await Process(pointAwarding.Points, pointAwarding.Co2Saving, CurrentUser);
        }

        private async Task Process(int points, double co2saving, User user)
        {
            user.Points += points;
            user.Co2Saving += co2saving;

            await UserRepository.UpdateAsync(CurrentUser.Id, user);

            // Fire and forget.
            _ = _eventStream.PublishAsync(new PointsReceivedEvent(user));
            _ = _eventStream.PublishAsync(new FriendPointsReceivedEvent(user));

            await _awardService.CheckForNewAwardsAsync(user);
        }

        public async Task<IEnumerable<PointAction>> PointHistory(string userId)
        {
            var user = await UserRepository.GetByIdAsync(userId);
            return user.PointActions;
        }
    }
}
