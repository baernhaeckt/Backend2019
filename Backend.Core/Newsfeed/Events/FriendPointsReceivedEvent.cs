using Backend.Database;

namespace Backend.Core.Newsfeed
{
    public class FriendPointsReceivedEvent : FriendEvent
    {
        public FriendPointsReceivedEvent(User user) :
            base(user)
        {
            title = "Freund Punkte";
            message = "Hei, einer deiner Freunde hat Punkte erhalten!";
        }
    }
}