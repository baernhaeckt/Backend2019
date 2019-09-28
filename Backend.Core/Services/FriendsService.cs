using Backend.Database;
using Backend.Database.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Backend.Core.Services
{
    public class FriendsService : PersonalizedService
    {
        public FriendsService(IUnitOfWork unitOfWork, ClaimsPrincipal principal)
            : base(unitOfWork, principal)
        { }

        public async Task AddFriend(string friendEmail)
        {
            await ConnectFriends(CurrentUser.Id, friendEmail);
        }

        public async Task ConnectFriends(Guid userId, string friendEmail)
        {
            if (string.IsNullOrEmpty(friendEmail))
            {
                throw new WebException("Email must not be empty", System.Net.HttpStatusCode.BadRequest);
            }

            User friendUser = await UnitOfWork.GetByEmailAsync(friendEmail);
            if (friendUser == null)
            {
                // TODO: Invite to Platform
                throw new WebException($"No user with email: {friendEmail} found.", System.Net.HttpStatusCode.BadRequest);
            }

            if (userId == friendUser.Id)
            {
                throw new WebException($"Can't be your own friend.", System.Net.HttpStatusCode.BadRequest);
            }

            var user = await UnitOfWork.GetAsync<User>(userId);
            if (user.Friends.Contains(friendUser.Id))
            {
                throw new WebException("User is already your friend", System.Net.HttpStatusCode.BadRequest);
            }

            user.Friends.Add(friendUser.Id);
            await UnitOfWork.UpdateAsync(user);

            var friendsFriend = (friendUser.Friends ?? Enumerable.Empty<Guid>()).ToList();
            if (!friendsFriend.Contains(user.Id))
            {
                friendsFriend.Add(user.Id);
                friendUser.Friends = friendsFriend;
                await UnitOfWork.UpdateAsync(friendUser);
            }
        }

        public async Task RemoveFriend(Guid friendUserId)
        {
            User user = CurrentUser;

            var friends = user.Friends.ToList();
            if (!friends.Contains(friendUserId))
            {
                throw new WebException("User is not your friend", System.Net.HttpStatusCode.BadRequest);
            }

            friends.Remove(friendUserId);
            user.Friends = friends;
            await UnitOfWork.UpdateAsync(user);

            User exFriend = await UnitOfWork.GetAsync<User>(friendUserId);

            if (exFriend.Friends.Contains(CurrentUser.Id))
            {
                exFriend.Friends.Remove(CurrentUser.Id);
                await UnitOfWork.UpdateAsync(exFriend);
            }
        }

        public async Task<IEnumerable<User>> GetFriends()
        {
            if (CurrentUser.Friends == null)
            {
                return Enumerable.Empty<User>();
            }

            IList<User> friends = new List<User>(CurrentUser.Friends.Count);
            foreach (var friend in CurrentUser.Friends)
            {
                friends.Add(await UnitOfWork.GetAsync<User>(friend));
            }

            return friends;
        }
    }
}
