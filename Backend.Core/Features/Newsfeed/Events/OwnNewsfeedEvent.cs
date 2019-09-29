using Backend.Core.Entities;

namespace Backend.Core.Features.Newsfeed.Events
{
    public abstract class OwnNewsfeedEvent : NewsfeedEvent
    {
        protected OwnNewsfeedEvent(User user)
        {
            Audience.Add(user.Id);
            Variant = "success";
        }
    }
}