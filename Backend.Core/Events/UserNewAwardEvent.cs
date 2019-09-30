using Backend.Core.Entities;
using Backend.Core.Entities.Awards;
using Silverback.Messaging.Messages;

namespace Backend.Core.Events
{
    public class UserNewAwardEvent : IEvent
    {
        public UserNewAwardEvent(User user, Award award)
        {
            User = user;
            Award = award;
        }

        public User User { get; }

        public Award Award { get; }
    }
}