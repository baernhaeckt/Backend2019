using System.Security.Claims;
using System.Threading.Tasks;
using AspNetCore.MongoDB;
using Backend.Core.Security.Extensions;
using Backend.Database;

namespace Backend.Core.Services
{
    public class UserService : PersonalizedService
    {
        public UserService(IMongoOperation<User> userRepository, ClaimsPrincipal principal) 
            : base(userRepository, principal)
        {
        }

        public new User CurrentUser => base.CurrentUser;

        public async Task Update(Models.UserUpdateRequest updateUserRequest)
        {
            var user = CurrentUser;
            user.DisplayName = updateUserRequest.DisplayName;
            await UserRepository.UpdateAsync(user.Id, user);
        }

        public async Task AddPoints(Token token)
        {
            User user = await UserRepository.GetByIdAsync(Principal.Id());
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

            await UserRepository.UpdateAsync(CurrentUser.Id, user);
        }
    }
}
