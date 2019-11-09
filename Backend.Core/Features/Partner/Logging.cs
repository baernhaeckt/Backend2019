using System;
using Microsoft.Extensions.Logging;

namespace Backend.Core.Features.Partner
{
    public static class Logging
    {
        public static void PartnerSignInInitiated(this ILogger logger, Guid id)
        {
            logger.LogInformation(new EventId(1, typeof(Logging).Namespace), "Process sign in request for {id}.", id);
        }

        public static void PartnerSignInPartnerNotFound(this ILogger logger, Guid id)
        {
            logger.LogInformation(new EventId(2, typeof(Logging).Namespace), "No partner with id {id} found.", id);
        }

        public static void PartnerSignInPasswordMismatch(this ILogger logger, Guid id)
        {
            logger.LogInformation(new EventId(3, typeof(Logging).Namespace), "Password mismatch for partner {id}.", id);
        }

        public static void PartnerSignInSuccessful(this ILogger logger, Guid id, string name)
        {
            logger.LogInformation(new EventId(4, typeof(Logging).Namespace), "Successful sign in from partner {name} ({id}).", id, name);
        }
    }
}