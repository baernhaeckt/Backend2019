using System;
using Silverback.Messaging.Messages;

namespace Backend.Core.Features.Partner.Commands
{
    public class RewardUserTokenCommand : ICommand
    {
        public RewardUserTokenCommand(Guid tokenId, Guid userId)
        {
            TokenId = tokenId;
            UserId = userId;
        }

        public Guid TokenId { get; }

        public Guid UserId { get; }
    }
}