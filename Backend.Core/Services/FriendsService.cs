using AspNetCore.MongoDB;
using Backend.Core.Security.Extensions;
using Backend.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Backend.Core.Services
{
    public class FriendsService : PersonalizedService
    {
        public FriendsService(IMongoOperation<User> userResponseRepository, ClaimsPrincipal principal)
            : base(userResponseRepository, principal)
        { }

        public async Task AddFriend(string friendEmail)
        {
            if (String.IsNullOrEmpty(friendEmail))
            {
                throw new WebException("email must not be empty", System.Net.HttpStatusCode.BadRequest);
            }

            User friendUser = UserRepository.GetQuerableAsync().SingleOrDefault(u => u.Email == Principal.Email());

            if (friendUser == null)
            {
                // TODO: Invite to Platform
                throw new WebException($"no user with email: {friendEmail} found.", System.Net.HttpStatusCode.BadRequest);
            }

            var friends = (CurrentUser.Friends ?? Enumerable.Empty<string>()).ToList();
            if (friends.Contains(friendUser.Id))
            {
                throw new WebException("user is already your friend", System.Net.HttpStatusCode.BadRequest);
            }
            
            friends.Add(friendUser.Id);
            var user = CurrentUser;
            user.Friends = friends;

            await UserRepository.UpdateAsync(user.Id, user);
        }

        public async Task RemoveFriend(string friendUserId)
        {
            User user = UserRepository.GetQuerableAsync()
                .Single(u => u.Email.Equals(Principal.Email(), StringComparison.InvariantCultureIgnoreCase));

            var friends = user.Friends.ToList();
            if (!friends.Contains(friendUserId))
            {
                throw new WebException("user is not your friend", System.Net.HttpStatusCode.BadRequest);
            }

            friends.Remove(friendUserId);
            user.Friends = friends;

            await UserRepository.UpdateAsync(user.Id, user);
        }

        public IEnumerable<User> Friends
        {
            get
            {
                if (CurrentUser.Friends == null)
                {
                    return Enumerable.Empty<User>();
                }

                return CurrentUser.Friends.Select(refId => UserRepository.GetByIdAsync(refId.ToString()).Result);
            }
        }
    }
}
