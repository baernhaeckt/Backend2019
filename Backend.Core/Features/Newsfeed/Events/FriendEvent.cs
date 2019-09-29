using Backend.Database;

namespace Backend.Core.Features.Newsfeed.Events
{
    public abstract class FriendEvent : Event
    {
        public FriendEvent(User user)
        {
            variant = "info";
            foreach (var friend in user.Friends)
            {
                Audience.Add(friend);
            }
        }
    }
}