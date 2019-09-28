using Backend.Core.Newsfeed;
using Backend.Core.Security;
using Backend.Core.Security.Abstraction;
using Backend.Database;
using Backend.Database.Abstraction;
using Backend.Models;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Backend.Core.Services
{
    public class UserService : PersonalizedService
    {
        private readonly IEventStream _eventStream;
        private readonly ISecurityTokenFactory _securityTokenFactory;
        private readonly IPasswordGenerator _passwordGenerator;
        private readonly IPasswordStorage _passwordStorage;
        private readonly AwardService _awardService;

        public UserService(IUnitOfWork unitOfWork, ClaimsPrincipal principal, IEventStream eventStream, ISecurityTokenFactory securityTokenFactory, IPasswordGenerator passwordGenerator, IPasswordStorage passwordStorage, AwardService awardService)
            : base(unitOfWork, principal)
        {
            _eventStream = eventStream;
            _securityTokenFactory = securityTokenFactory;
            _passwordGenerator = passwordGenerator;
            _passwordStorage = passwordStorage;
            _awardService = awardService;
        }

        public async Task Update(UserUpdateRequest updateUserRequest)
        {
            var user = CurrentUser;
            user.DisplayName = updateUserRequest.DisplayName;
            await UnitOfWork.UpdateAsync(user);
        }

        public async Task AddPoints(Token token)
        {
            var user = CurrentUser;
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

            await Process(token.Points, token.Co2Saving, user);
        }

        public async Task<User> GetByEmailAsync(string email) => await UnitOfWork.GetByEmailAsync(email);

        public async Task<IEnumerable<User>> GetByPlzAsync(string zip) => await UnitOfWork.GetByZipAsync(zip);

        public async Task<bool> IsRegisteredAsync(string email) => (await UnitOfWork.GetByEmailAsync(email) != null);

        public async Task<string> RegisterAsync(string email)
        {
            string newPassword = _passwordGenerator.Generate();
            var newUser = new User
            {
                Email = email,
                Password = _passwordStorage.Create(newPassword),
                DisplayName = "ÖkoRookie",
                Roles = new List<string> { Roles.User },
                Location = new Location
                {
                    City = "Bern",
                    Zip = "3011",
                    Latitude = 46.944699,
                    Longitude = 7.443788
                }
            };
            newUser = await UnitOfWork.InsertAsync(newUser);

            string token = _securityTokenFactory.Create(newUser);
            return token;
        }

        public async Task<IEnumerable<User>> GetAllAsync() => await UnitOfWork.GetAllAsync<User>();

        public async Task AddPoints(PointAwarding pointAwarding)
        {
            var user = CurrentUser;
            user.PointActions.Add(new PointAction
            {
                Point = pointAwarding.Points,
                Action = pointAwarding.Text,
                Co2Saving = pointAwarding.Co2Saving,
                SponsorRef = pointAwarding.Source.ToString(),
                SufficientType = new UserSufficientType
                {
                    Title = "Wissen"
                }
            });

            await Process(pointAwarding.Points, pointAwarding.Co2Saving, user);
        }

        private async Task Process(int points, double co2saving, User user)
        {
            user.Points += points;
            user.Co2Saving += co2saving;

            await UnitOfWork.UpdateAsync(user);

            // Fire and forget.
            await _eventStream.PublishAsync(new PointsReceivedEvent(user, points));
            await _eventStream.PublishAsync(new FriendPointsReceivedEvent(user, points));

            await _awardService.CheckForNewAwardsAsync(user);
        }

        public async Task<IEnumerable<PointAction>> PointHistory(Guid id)
        {
            var user = await UnitOfWork.GetAsync<User>(id);
            return user.PointActions;
        }
    }
}
