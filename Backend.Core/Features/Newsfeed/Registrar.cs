using Backend.Core.Features.Newsfeed.Abstraction;
using Backend.Core.Features.Newsfeed.EventHandler;
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
            services.AddScoped<ISubscriber, UserNewAwardEventHandler>();

            return services;
        }
    }
}