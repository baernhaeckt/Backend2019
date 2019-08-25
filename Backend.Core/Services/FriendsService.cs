using AspNetCore.MongoDB;
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
            await ConnectFriends(CurrentUser.Id, friendEmail);
        }

        public async Task ConnectFriends(string userId, string friendEmail)
        {
            if (String.IsNullOrEmpty(friendEmail))
            {
                throw new WebException("email must not be empty", System.Net.HttpStatusCode.BadRequest);
            }

            User friendUser = UserRepository.GetQuerableAsync().SingleOrDefault(u => u.Email == friendEmail);

            if (friendUser == null)
            {
                // TODO: Invite to Platform
                throw new WebException($"No user with email: {friendEmail} found.", System.Net.HttpStatusCode.BadRequest);
            }

            if (userId == friendUser.Id)
            {
                throw new WebException($"can't be your own friend.", System.Net.HttpStatusCode.BadRequest);
            }

            var user = await UserRepository.GetByIdAsync(userId);
            var friends = (user.Friends ?? Enumerable.Empty<string>()).ToList();
            if (friends.Contains(friendUser.Id))
            {
                throw new WebException("user is already your friend", System.Net.HttpStatusCode.BadRequest);
            }

            friends.Add(friendUser.Id);
            user.Friends = friends;
            await UserRepository.UpdateAsync(user.Id, user);

            var friendsFriend = (friendUser.Friends ?? Enumerable.Empty<string>()).ToList();
            if (!friendsFriend.Contains(user.Id))
            {
                friendsFriend.Add(user.Id);
                friendUser.Friends = friendsFriend;
                await UserRepository.UpdateAsync(friendUser.Id, friendUser);
            }
        }

        public async Task RemoveFriend(string friendUserId)
        {
            User user = CurrentUser;

            var friends = user.Friends.ToList();
            if (!friends.Contains(friendUserId))
            {
                throw new WebException("User is not your friend", System.Net.HttpStatusCode.BadRequest);
            }

            friends.Remove(friendUserId);
            user.Friends = friends;
            await UserRepository.UpdateAsync(user.Id, user);

            User exFriend = await UserRepository.GetByIdAsync(friendUserId);

            var exFriendFriends = exFriend.Friends.ToList();
            if (exFriendFriends.Contains(CurrentUser.Id))
            {
                exFriendFriends.Remove(CurrentUser.Id);
                exFriend.Friends = exFriendFriends;
                await UserRepository.UpdateAsync(exFriend.Id, exFriend);
            }
        }

        public IEnumerable<User> GetFriends()
        {
            if (CurrentUser.Friends == null)
            {
                return Enumerable.Empty<User>();
            }

            return CurrentUser.Friends.Select(refId => UserRepository.GetByIdAsync(refId.ToString()).Result).ToList();
        }
    }
}
