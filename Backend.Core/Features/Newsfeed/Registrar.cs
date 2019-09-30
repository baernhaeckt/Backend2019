using Backend.Core.Features.Awards.EventHandler;
using Backend.Core.Features.Newsfeed.Abstraction;
using Microsoft.Extensions.DependencyInjection;
using Silverback.Messaging.Subscribers;

namespace Backend.Core.Features.Newsfeed
{
    public static class Registrar
    {
        public static IServiceCollection AddFeatureNewsfeed(this IServiceCollection services)
        {
            services.AddTransient<IEventFeed, SignalREventFeed>();
            services.AddSignalR();

            // EventHandlers
            services.AddScoped<ISubscriber, UserNewPointsEventHandler>();

            return services;
        }
    }
}