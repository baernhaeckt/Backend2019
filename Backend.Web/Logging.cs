using Microsoft.Extensions.Logging;

namespace Backend.Web
{
    public static class Logging
    {
        public static void EntityNotFound(this ILogger logger, string name, string methodName, string filter)
        {
            logger.LogInformation(new EventId(1, typeof(Logging).Namespace), "Entity not found. {name} {methodName} {filter}", name, methodName, filter);
        }

        public static void ValidationException(this ILogger logger, string message)
        {
            logger.LogInformation(new EventId(2, typeof(Logging).Namespace), "Validation Exception. {message}", message);
        }
    }
}