using Backend.Database;

namespace Backend.Core.Newsfeed
{
    public class BadgeReceivedEvent : OwnEvent
    {
        public BadgeReceivedEvent(User user)
            : base(user)
        {
            title = "Award erhalten";
            message = "Gratulation, du hast einen neuen Award erhalten!";
        }
    }
}