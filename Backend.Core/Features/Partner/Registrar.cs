using Backend.Core.Features.Partner.Commands;
using Backend.Core.Features.Partner.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Silverback.Messaging.Subscribers;

namespace Backend.Core.Features.Partner
{
    public static class Registrar
    {
        public static IServiceCollection AddFeaturePartner(this IServiceCollection services)
        {
            // Controllers
            services.AddScoped<TokensController>();

            // Services
            services.AddScoped<TokenService>();

            // Commands
            services.AddScoped<ISubscriber, RewardForUserTokenCommandHandler>();

            return services;
        }
    }
}