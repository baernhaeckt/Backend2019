using Backend.Core.Extensions;
using Backend.Core.Features.Partner.Commands;
using Backend.Core.Features.Partner.Controllers;
using Backend.Core.Features.Partner.Data.Testing;
using Backend.Core.Features.Partner.Queries;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Backend.Core.Features.Partner
{
    public static class Registrar
    {
        public static IServiceCollection AddFeaturePartner(this IServiceCollection services, IHostEnvironment hostEnvironment)
        {
            // Controllers
            services.AddScoped<TokensController>();

            // Commands
            services.AddScopedSubscriber<RewardUserTokenCommandHandler>();
            services.AddScopedSubscriber<CreateNewTokenCommandHandler>();

            // Queries
            services.AddScopedSubscriber<SignInQueryHandler>();

            if (hostEnvironment.IsDevelopment())
            {
                services.AddStartupTask<GenerateTokenIssuersStartupTask>();
            }

            return services;
        }
    }
}