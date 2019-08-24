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

        public async Task AddFriend(Guid friendUserId)
        {
            User user = UserRepository.GetQuerableAsync()
                .Single(u => u.Email.Equals(Principal.Email(), StringComparison.InvariantCultureIgnoreCase));

            var friends = (user.Friends ?? Enumerable.Empty<Guid>()).ToList();
            if (friends.Contains(friendUserId))
            {
                throw new WebException("user is already your friend", System.Net.HttpStatusCode.BadRequest);
            }

            if (UserRepository.GetQuerableAsync().All(u => u.Id != friendUserId.ToString()))
            {
                throw new WebException("friend couldn't be found", System.Net.HttpStatusCode.NotFound);
            }

            friends.Add(friendUserId);
            user.Friends = friends;

            await UserRepository.UpdateAsync(user.Id, user);
        }

        public async Task RemoveFriend(Guid friendUserId)
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
