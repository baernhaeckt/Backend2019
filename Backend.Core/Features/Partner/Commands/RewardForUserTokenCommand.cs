using System;
using Silverback.Messaging.Messages;

namespace Backend.Core.Features.Partner.Commands
{
    public class RewardForUserTokenCommand : ICommand
    {
        public RewardForUserTokenCommand(Guid tokenId, Guid userId)
        {
            TokenId = tokenId;
            UserId = userId;
        }

        public Guid TokenId { get; }

        public Guid UserId { get; }
    }
}