using Backend.Core.Entities;
using Backend.Core.Entities.Awards;

namespace Backend.Core.Features.Newsfeed.Events
{
    public class BadgeReceivedNewsfeedEvent : OwnNewsfeedEvent
    {
        public BadgeReceivedNewsfeedEvent(User user, Award award)
            : base(user)
        {
            Title = "Award erhalten";
            Message = "Gratulation, du hast den Award '" + award.Name + "' erhalten!";
        }
    }
}