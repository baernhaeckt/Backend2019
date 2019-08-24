namespace Backend.Core.Newsfeed
{
    public class FriendBadgeReceivedEvent : Event
    {
        public FriendBadgeReceivedEvent()
        {
            title = "Freund Award";
            message = "Hei, einer deiner Freunde hat einen Award erhalten!";
            variant = "info";
        }
    }
}