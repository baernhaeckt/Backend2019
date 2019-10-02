using Microsoft.Extensions.Logging;

namespace Backend.Infrastructure.Email
{
    public static class Logging
    {
        public static void SendEmail(this ILogger logger, string subject, string text, string receiver)
        {
            logger.LogInformation(new EventId(1), typeof(Logging).Namespace, "Send E-mail.", subject, text, receiver);
        }
    }
}