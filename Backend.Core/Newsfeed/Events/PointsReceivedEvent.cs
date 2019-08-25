using Backend.Database;

namespace Backend.Core.Newsfeed
{
    public class PointsReceivedEvent : OwnEvent
    {
        public PointsReceivedEvent(User user, int points)
            : base(user)
        {
            title = "Punkte erhalten";
            message = "Gratulation, du hast " + points + " neue Punkte erhalten!";
        }
    }
}