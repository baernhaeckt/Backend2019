using System;
using Silverback.Messaging.Messages;

namespace Backend.Core.Features.Partner.Commands
{
    public class CreateNewTokenCommand : ICommand<Guid>
    {
        public CreateNewTokenCommand(Guid partnerId, string tokenType)
        {
            PartnerId = partnerId;
            TokenType = tokenType;
        }

        public Guid PartnerId { get; }

        public string TokenType { get; }
    }
}