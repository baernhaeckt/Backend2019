using System;

namespace Backend.Core.Features.Newsfeed.Events
{
    public class AwardReceivedNewsfeedEvent : OwnNewsfeedEvent
    {
        public AwardReceivedNewsfeedEvent(Guid userId, string awardName)
            : base(userId)
        {
            Title = "Award erhalten";
            Message = "Gratulation, du hast den Award '" + awardName + "' erhalten!";
        }
    }
}