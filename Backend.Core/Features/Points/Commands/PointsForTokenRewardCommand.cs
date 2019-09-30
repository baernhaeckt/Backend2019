using System;
using Silverback.Messaging.Messages;

namespace Backend.Core.Features.Points.Commands
{
    public class PointsForTokenRewardCommand : ICommand
    {
        public PointsForTokenRewardCommand(Guid token)
        {
            Token = token;
        }

        public Guid Token { get; }
    }
}