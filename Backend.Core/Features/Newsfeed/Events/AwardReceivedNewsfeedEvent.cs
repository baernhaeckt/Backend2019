using Backend.Core.Entities;
using Backend.Core.Entities.Awards;

namespace Backend.Core.Features.Newsfeed.Events
{
    public class AwardReceivedNewsfeedEvent : OwnNewsfeedEvent
    {
        public AwardReceivedNewsfeedEvent(User user, Award award)
            : base(user)
        {
            Title = "Award erhalten";
            Message = "Gratulation, du hast den Award '" + award.Name + "' erhalten!";
        }
    }
}