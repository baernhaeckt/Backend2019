using System;
using Silverback.Messaging.Messages;

namespace Backend.Core.Events
{
    public class PartnerTokenRegisteredEvent : IEvent
    {
        public PartnerTokenRegisteredEvent(Guid userId, Guid tokenId)
        {
            UserId = userId;
            TokenId = tokenId;
        }

        public Guid UserId { get; }

        public Guid TokenId { get; }
    }
}