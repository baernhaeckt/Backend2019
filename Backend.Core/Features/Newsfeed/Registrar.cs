using Backend.Core.Features.Newsfeed.Abstraction;
using Backend.Core.Features.Newsfeed.EventHandler;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.Core.Features.Newsfeed
{
    public static class Registrar
    {
        public static IServiceCollection AddFeatureNewsfeed(this IServiceCollection services)
        {
            services.AddTransient<IEventFeed, SignalREventFeed>();
            services.AddSignalR();

            // EventHandlers
            services.AddScopedSubscriber<UserNewPointsEventHandler>();
            services.AddScopedSubscriber<UserNewAwardEventHandler>();

            return services;
        }
    }
}