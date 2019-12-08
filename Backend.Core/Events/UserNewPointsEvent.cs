using System;
using Silverback.Messaging.Messages;

namespace Backend.Core.Events
{
    public class UserNewPointsEvent : IEvent
    {
        public UserNewPointsEvent(Guid userId, int points, double co2Saving, string userDisplayName)
        {
            UserId = userId;
            Points = points;
            Co2Saving = co2Saving;
            UserDisplayName = userDisplayName;
        }

        public Guid UserId { get; }

        public int Points { get; }

        public double Co2Saving { get; }

        public string UserDisplayName { get; }
    }
}