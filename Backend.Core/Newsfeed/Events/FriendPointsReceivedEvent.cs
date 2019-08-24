namespace Backend.Core.Newsfeed
{
    public class FriendPointsReceivedEvent : Event
    {
        public FriendPointsReceivedEvent()
        {
            title = "Freund Punkte";
            message = "Hei, einer deiner Freunde hat Punkte erhalten!";
            variant = "info";
        }
    }
}