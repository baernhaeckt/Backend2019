using Backend.Database;

namespace Backend.Core.Features.Newsfeed.Events
{
    public abstract class OwnEvent : Event
    {
        public OwnEvent(User user)
        {
            Audience.Add(user.Id);
            variant = "success";
        }
    }
}