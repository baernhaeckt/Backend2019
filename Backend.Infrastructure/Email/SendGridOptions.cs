namespace Backend.Infrastructure.Email
{
    public class SendGridOptions
    {
        public string SenderDisplayName { get; set; } = string.Empty;

        public string SenderEmail { get; set; } = string.Empty;

        public string ApiKey { get; set; } = string.Empty;
    }
}