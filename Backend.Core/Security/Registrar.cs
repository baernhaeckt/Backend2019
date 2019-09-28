using Backend.Core.Security.Abstraction;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Backend.Core.Security
{
    public static class Registrar
    {
        public static IServiceCollection AddFeatureLogin(this IServiceCollection services, IHostEnvironment hostEnvironment)
        {
            services.AddSingleton<IPasswordStorage, HmacSha512PasswordStorage>();
            services.AddSingleton<ISecurityTokenFactory, JwtSecurityTokenFactory>();

            services.AddSingleton<ISecurityKeyProvider, SymmetricSecurityKeyProvider>();

            if (hostEnvironment.IsDevelopment())
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
