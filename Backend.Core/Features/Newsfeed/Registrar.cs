using Backend.Core.Features.Newsfeed.Abstraction;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.Core.Features.Newsfeed
{
    public static class Registrar
    {
        public static IServiceCollection AddFeatureNewsfeed(this IServiceCollection services)
        {
            services.AddTransient<IEventFeed, SignalREventFeed>();
            services.AddSignalR();

            return services;
        }
    }
}