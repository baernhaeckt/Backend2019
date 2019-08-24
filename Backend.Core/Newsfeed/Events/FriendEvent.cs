using Backend.Database;
using System.Linq;

namespace Backend.Core.Newsfeed
{
    public abstract class FriendEvent : Event
    {
        public FriendEvent(User user)
        {
            variant = "info";
            foreach (var friend in user.Friends ?? Enumerable.Empty<string>())
            {
                Audience.Add(friend);
            }
        }
    }
}