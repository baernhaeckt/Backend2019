using System;
using Silverback.Messaging.Messages;

namespace Backend.Core.Events
{
    public class UserNewAwardEvent : IEvent
    {
        public UserNewAwardEvent(Guid userId, string userDisplayName, string awardName)
        {
            UserId = userId;
            UserDisplayName = userDisplayName;
            AwardName = awardName;
        }

        public Guid UserId { get; }

        public string UserDisplayName { get; }

        public string AwardName { get; }
    }
}