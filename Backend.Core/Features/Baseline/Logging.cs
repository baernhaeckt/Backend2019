using System;
using Microsoft.Extensions.Logging;

namespace Backend.Core.Features.Baseline
{
    public static class Logging
    {
        public static void RetrievePointsPerSufficientTypes(this ILogger logger, Guid userId)
        {
            logger.LogInformation(new EventId(1, typeof(Logging).Namespace), "Retrieve points per sufficient type for an user. User: {userId}", userId);
        }

        public static void RetrievePointsPerSufficientTypesSuccessful(this ILogger logger, Guid userId)
        {
            logger.LogInformation(new EventId(2, typeof(Logging).Namespace), "Retrieved points per sufficient type. User: {userId}", userId);
        }

        public static void RetrieveAllSufficientTypes(this ILogger logger)
        {
            logger.LogInformation(new EventId(3, typeof(Logging).Namespace), "Retrieve all sufficient types.");
        }

        public static void RetrieveAllSufficientTypesSuccessful(this ILogger logger)
        {
            logger.LogInformation(new EventId(4, typeof(Logging).Namespace), "Retrieved all sufficient types");
        }
    }
}