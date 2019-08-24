namespace Backend.Core.Newsfeed
{
    public class PointsReceivedEvent : Event
    {
        public PointsReceivedEvent()
        {
            title = "Punkte erhalten";
            message = "Gratulation, du hast neue Punkte erhalten!";
            variant = "success";
        }
    }
}