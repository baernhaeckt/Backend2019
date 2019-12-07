using Backend.Infrastructure.Abstraction.Hosting;
using Backend.Infrastructure.Abstraction.Security;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Backend.Infrastructure.Security
{
    public static class Registrar
    {
        public static IServiceCollection AddSecurity(this IServiceCollection services, IHostEnvironment hostEnvironment)
        {
            // Security Utilities
            services.AddSingleton<IPasswordStorage, HmacSha512PasswordStorage>();
            services.AddSingleton<ISecurityTokenFactory, JwtSecurityTokenFactory>();
            services.AddSingleton<ISecurityKeyProvider, SymmetricSecurityKeyProvider>();
            if (hostEnvironment.IsIntegrationTestOrDevelopment())
            {
                services.AddSingleton<IPasswordGenerator, StaticPasswordGenerator>();
            }
            else
            {
                services.AddSingleton<IPasswordGenerator, RandomPasswordGenerator>();
            }

            return services;
        }
    }
}