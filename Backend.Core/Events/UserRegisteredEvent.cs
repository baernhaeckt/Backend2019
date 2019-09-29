using Backend.Core.Entities;
using Silverback.Messaging.Messages;

namespace Backend.Core.Events
{
    public class UserRegisteredEvent : IEvent
    {
        public UserRegisteredEvent(User user, string plainTextPassword)
        {
            User = user;
            PlainTextPassword = plainTextPassword;
        }

        public string PlainTextPassword { get; }

        public User User { get; }
    }
}
