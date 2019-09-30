using Backend.Core.Features.Friendship.Controllers;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.Core.Features.Friendship
{
    public static class Registrar
    {
        public static IServiceCollection AddFeatureFriendship(this IServiceCollection services)
        {
            // Controllers
            services.AddScoped<FriendsController>();

            // Services
            services.AddScoped<FriendsService>();

            return services;
        }
    }
}