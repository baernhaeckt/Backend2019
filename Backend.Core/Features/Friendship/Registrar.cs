using Backend.Core.Features.Friendship.Controllers;
using Backend.Core.Features.Friendship.Queries;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.Core.Features.Friendship
{
    public static class Registrar
    {
        public static IServiceCollection AddFeatureFriendship(this IServiceCollection services)
        {
            // Controllers
            services.AddScoped<FriendsController>();

            // CommandHandlers
            services.AddScoped<FriendsService>();

            // QueryHandlers
            services.AddScopedSubscriber<FriendsQueryHandler>();

            return services;
        }
    }
}