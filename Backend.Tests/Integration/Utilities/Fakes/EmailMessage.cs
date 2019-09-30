namespace Backend.Tests.Integration.Utilities.Fakes
{
    public class EmailMessage
    {
        public EmailMessage(string subject, string text, string receiver)
        {
            Subject = subject;
            Text = text;
            Receiver = receiver;
        }

        public string Subject { get; }

        public string Text { get; }

        public string Receiver { get; }
    }
}