using Backend.Core.Features.Partner.Controllers;
using Microsoft.Extensions.DependencyInjection;

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

            return services;
        }
    }
}