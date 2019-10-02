using Backend.Core.Features.Awards.Controllers;
using Backend.Core.Features.Awards.EventHandler;
using Backend.Core.Features.Awards.Queries;
using Microsoft.Extensions.DependencyInjection;
using Silverback.Messaging.Subscribers;

namespace Backend.Core.Features.Awards
{
    public static class Registrar
    {
        public static IServiceCollection AddFeatureAward(this IServiceCollection services)
        {
            // Controllers
            services.AddScoped<AwardsController>();

            // QueryHandlers
            services.AddScoped<ISubscriber, UserAwardsQueryQueryHandler>();

            // EventHandlers
            services.AddScoped<ISubscriber, UserNewPointsEventHandler>();

            return services;
        }
    }
}