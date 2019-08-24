using Backend.Database;

namespace Backend.Core.Newsfeed
{
    public class FriendBadgeReceivedEvent : FriendEvent
    {
        public FriendBadgeReceivedEvent(User user) :
            base(user)
        {
            title = "Freund Award";
            message = "Hei, einer deiner Freunde hat einen Award erhalten!";
        }
    }
}