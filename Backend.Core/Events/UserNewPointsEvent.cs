using Backend.Core.Entities;
using Silverback.Messaging.Messages;

namespace Backend.Core.Events
{
    public class UserNewPointsEvent : IEvent
    {
        public UserNewPointsEvent(User user, int points, double co2Saving)
        {
            User = user;
            Points = points;
            Co2Saving = co2Saving;
        }

        public User User { get; }

        public int Points { get; }

        public double Co2Saving { get; }
    }
}