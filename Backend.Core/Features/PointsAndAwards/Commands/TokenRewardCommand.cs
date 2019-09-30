using System;
using Silverback.Messaging.Messages;

namespace Backend.Core.Features.PointsAndAwards.Commands
{
    public class TokenRewardCommand : ICommand
    {
        public TokenRewardCommand(Guid token)
        {
            Token = token;
        }

        public Guid Token { get; }
    }
}