using System;

namespace Backend.Core.Features.Newsfeed.Events
{
    public class PointsReceivedNewsfeedEvent : OwnNewsfeedEvent
    {
        public PointsReceivedNewsfeedEvent(Guid userId, int points)
            : base(userId)
        {
            Title = "Punkte erhalten";
            Message = "Gratulation, du hast " + points + " neue Punkte erhalten!";
        }
    }
}