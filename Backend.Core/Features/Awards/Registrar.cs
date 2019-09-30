using Backend.Core.Features.Awards.Controllers;
using Backend.Core.Features.Awards.EventHandler;
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

            // EventHandlers
            services.AddScoped<ISubscriber, UserNewPointsEventHandler>();

            return services;
        }
    }
}