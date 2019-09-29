using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Backend.Core.Extensions;
using Backend.Database;
using Backend.Database.Abstraction;

namespace Backend.Core.Features.Friendship
{
    public class FriendsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ClaimsPrincipal _principal;

        public FriendsService(IUnitOfWork unitOfWork, ClaimsPrincipal principal)
        {
            _unitOfWork = unitOfWork;
            _principal = principal;
        }

        public async Task AddFriend(string friendEmail)
        {
            await ConnectFriends(_principal.Id(), friendEmail);
        }

        public async Task ConnectFriends(Guid userId, string friendEmail)
        {
            if (string.IsNullOrEmpty(friendEmail))
            {
                throw new WebException("Email must not be empty", System.Net.HttpStatusCode.BadRequest);
            }

            User friendUser = await _unitOfWork.GetByEmailAsync(friendEmail);
            if (friendUser == null)
            {
                // TODO: Invite to Platform
                throw new WebException($"No user with email: {friendEmail} found.", System.Net.HttpStatusCode.BadRequest);
            }

            if (userId == friendUser.Id)
            {
                throw new WebException($"Can't be your own friend.", System.Net.HttpStatusCode.BadRequest);
            }

            var user = await _unitOfWork.GetAsync<User>(userId);
            if (user.Friends.Contains(friendUser.Id))
            {
                throw new WebException("User is already your friend", System.Net.HttpStatusCode.BadRequest);
            }

            user.Friends.Add(friendUser.Id);
            await _unitOfWork.UpdateAsync(user);

            var friendsFriend = (friendUser.Friends ?? Enumerable.Empty<Guid>()).ToList();
            if (!friendsFriend.Contains(user.Id))
            {
                friendsFriend.Add(user.Id);
                friendUser.Friends = friendsFriend;
                await _unitOfWork.UpdateAsync(friendUser);
            }
        }

        public async Task RemoveFriend(Guid friendUserId)
        {
            var user = await _unitOfWork.GetAsync<User>(_principal.Id());

            var friends = user.Friends.ToList();
            if (!friends.Contains(friendUserId))
            {
                throw new WebException("User is not your friend", System.Net.HttpStatusCode.BadRequest);
            }

            friends.Remove(friendUserId);
            user.Friends = friends;
            await _unitOfWork.UpdateAsync(user);

            User exFriend = await _unitOfWork.GetAsync<User>(friendUserId);

            if (exFriend.Friends.Contains(user.Id))
            {
                exFriend.Friends.Remove(user.Id);
                await _unitOfWork.UpdateAsync(exFriend);
            }
        }

        public async Task<IEnumerable<User>> GetFriends()
        {
            var user = await _unitOfWork.GetAsync<User>(_principal.Id());
            if (user.Friends == null)
            {
                return Enumerable.Empty<User>();
            }

            IList<User> friends = new List<User>(user.Friends.Count);
            foreach (var friend in user.Friends)
            {
                friends.Add(await _unitOfWork.GetAsync<User>(friend));
            }

            return friends;
        }
    }
}
