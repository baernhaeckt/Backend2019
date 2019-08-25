using Backend.Database;

namespace Backend.Core.Newsfeed
{
    public class BadgeReceivedEvent : OwnEvent
    {
        public BadgeReceivedEvent(User user, Award award)
            : base(user)
        {
            title = "Award erhalten";
            message = "Gratulation, du hast den Award '" + award.Name + "' erhalten!";
        }
    }
}