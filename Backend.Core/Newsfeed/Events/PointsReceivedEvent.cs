using Backend.Database;

namespace Backend.Core.Newsfeed
{
    public class PointsReceivedEvent : OwnEvent
    {
        public PointsReceivedEvent(User user)
            : base(user)
        {
            title = "Punkte erhalten";
            message = "Gratulation, du hast neue Punkte erhalten!";
        }
    }
}