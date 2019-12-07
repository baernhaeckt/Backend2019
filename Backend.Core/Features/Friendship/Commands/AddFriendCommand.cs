using System;
using Silverback.Messaging.Messages;

namespace Backend.Core.Features.Friendship.Commands
{
    public class AddFriendCommand : ICommand
    {
        public AddFriendCommand(Guid userId, string friendEmail)
        {
            UserId = userId;
            FriendEmail = friendEmail;
        }

        public Guid UserId { get; }

        public string FriendEmail { get; }
    }
}