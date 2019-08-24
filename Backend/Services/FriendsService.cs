using AspNetCore.MongoDB;
using Backend.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Backend.Services
{
    public class FriendsService : PersonalizedService
    {
        public FriendsService(IMongoOperation<User> userResponseRepository, ClaimsPrincipal principal)
            : base(userResponseRepository, principal)
        { }

        public async Task AddFriend(Guid friendUserId)
        {
            User user = userRepository.GetQuerableAsync()
                .Single(u => u.Email.Equals(currentUserId, StringComparison.InvariantCultureIgnoreCase));

            var friends = (user.Friends ?? Enumerable.Empty<Guid>()).ToList();
            if (friends.Contains(friendUserId))
            {
                throw new Core.WebException("user is already your friend", System.Net.HttpStatusCode.BadRequest);
            }

            if (userRepository.GetQuerableAsync().All(u => u.Id != friendUserId.ToString()))
            {
                throw new Core.WebException("friend couldn't be found", System.Net.HttpStatusCode.NotFound);
            }

            friends.Add(friendUserId);
            user.Friends = friends;

            await userRepository.UpdateAsync(currentUserId, user);
        }

        public async Task RemoveFriend(Guid friendUserId)
        {
            User user = userRepository.GetQuerableAsync()
                .Single(u => u.Email.Equals(currentUserId, StringComparison.InvariantCultureIgnoreCase));

            var friends = user.Friends.ToList();
            if (!friends.Contains(friendUserId))
            {
                throw new Core.WebException("user is not your friend", System.Net.HttpStatusCode.BadRequest);
            }

            friends.Remove(friendUserId);
            user.Friends = friends;

            await userRepository.UpdateAsync(currentUserId, user);
        }

        public IEnumerable<User> Friends
        {
            get
            {
                if (CurrentUser.Friends == null)
                {
                    return Enumerable.Empty<User>();
                }

                return CurrentUser.Friends.Select(refId => userRepository.GetByIdAsync(refId.ToString()).Result);
            }
        }
    }
}
