using Backend.Core.Entities;
using Silverback.Messaging.Messages;

namespace Backend.Core.Events
{
    public class PartnerTokenRegisteredEvent : IEvent
    {
        public PartnerTokenRegisteredEvent(Token token)
        {
            Token = token;
        }

        public Token Token { get; }
    }
}