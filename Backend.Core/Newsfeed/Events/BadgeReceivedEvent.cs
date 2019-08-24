namespace Backend.Core.Newsfeed
{
    public class BadgeReceivedEvent : Event
    {
        public BadgeReceivedEvent()
        {
            title = "Award erhalten";
            message = "Gratulation, du hast einen neuen Award erhalten!";
            variant = "success";
        }
    }
}