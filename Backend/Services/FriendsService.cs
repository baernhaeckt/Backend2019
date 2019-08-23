using AspNetCore.MongoDB;
using Backend.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;

namespace Backend.Services
{
    public class FriendsService
    {
        private String currentUserId
        {
            get { return Principal.Identity.Name; }
        }

        public IMongoOperation<User> UserResponseRepository { get; }
        public IPrincipal Principal { get; }

        public FriendsService(IMongoOperation<User> userResponseRepository, IPrincipal principal)
        {
            UserResponseRepository = userResponseRepository;
            Principal = principal;
        }

        public async Task AddFriend(Guid friendUserId)
        {
            User user = UserResponseRepository.GetQuerableAsync()
                .Single(u => u.Email.Equals(currentUserId, StringComparison.InvariantCultureIgnoreCase));

            var friends = user.Friends.ToList();
            if (friends.Contains(friendUserId))
            {
                throw new Core.WebException("user is already your friend", System.Net.HttpStatusCode.BadRequest);
            }

            friends.Add(friendUserId);
            user.Friends = friends;

            await UserResponseRepository.UpdateAsync(currentUserId, user);
        }

        public async Task RemoveFriend(Guid friendUserId)
        {
            User user = UserResponseRepository.GetQuerableAsync()
                .Single(u => u.Email.Equals(currentUserId, StringComparison.InvariantCultureIgnoreCase));

            var friends = user.Friends.ToList();
            if (!friends.Contains(friendUserId))
            {
                throw new Core.WebException("user is not your friend", System.Net.HttpStatusCode.BadRequest);
            }

            friends.Remove(friendUserId);
            user.Friends = friends;

            await UserResponseRepository.UpdateAsync(currentUserId, user);
        }

        public IEnumerable<User> Friends
        {
            get
            {
                User user = UserResponseRepository.GetByIdAsync(currentUserId).Result;
                return user.Friends.Select(refId => UserResponseRepository.GetByIdAsync(refId.ToString()).Result);
            }
        }
    }
}
