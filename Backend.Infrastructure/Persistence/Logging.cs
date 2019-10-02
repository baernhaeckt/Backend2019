using Microsoft.Extensions.Logging;

namespace Backend.Infrastructure.Persistence
{
    public static class Logging
    {
        public static void Dummy(this ILogger logger, string parameter)
        {
            logger.LogInformation(new EventId(1), typeof(Logging).Namespace, "Dummy", parameter);
        }
    }
}
