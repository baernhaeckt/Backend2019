using System;
using Silverback.Messaging.Messages;

namespace Backend.Core.Features.Friendship.Commands
{
    public class RemoveFriendCommand : ICommand
    {
        public RemoveFriendCommand(Guid userId, Guid friendUserId)
        {
            UserId = userId;
            FriendUserId = friendUserId;
        }

        public Guid UserId { get; }

        public Guid FriendUserId { get; }
    }
}