namespace Backend.Core.Hubs
{
    public class Event
    {
        public string title { get; set; }

        public string message { get; set; }

        // success oder info
        public string variant { get; set; }
    }
}