using Microsoft.Extensions.Logging;

namespace Backend.Core.Features.Friendship
{
    public static class Logging
    {
        public static void Dummy(this ILogger logger, string parameter)
        {
            logger.LogInformation(new EventId(1), typeof(Logging).Namespace, "Dummy", parameter);
        }
    }
}
