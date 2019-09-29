using Backend.Core.Entities;

namespace Backend.Core.Features.Newsfeed.Events
{
    public class PointsReceivedNewsfeedEvent : OwnNewsfeedEvent
    {
        public PointsReceivedNewsfeedEvent(User user, int points)
            : base(user)
        {
            Title = "Punkte erhalten";
            Message = "Gratulation, du hast " + points + " neue Punkte erhalten!";
        }
    }
}