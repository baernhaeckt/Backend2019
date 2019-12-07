using Backend.Core.Features.Friendship.Commands;
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
            services.AddScopedSubscriber<AddFriendCommandHandler>();
            services.AddScopedSubscriber<RemoveFriendCommandCommandHandler>();

            // QueryHandlers
            services.AddScopedSubscriber<FriendsQueryHandler>();

            return services;
        }
    }
}