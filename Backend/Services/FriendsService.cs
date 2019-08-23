using AspNetCore.MongoDB;
using Backend.Models;
using Backend.Models.Database;
using System;

namespace Backend.Services
{
    public class FriendsService
    {

        public IMongoOperation<User> UserResponseRepository { get; }

        public FriendsService(IMongoOperation<User> userResponseRepository)
        {
            UserResponseRepository = userResponseRepository;
        }


        public bool AddFriend(Guid friendUserId)
        {
            return false;
        }

        public bool RemoveFriend(Guid friendUserId)
        {
            return false;
        }

    }
}
