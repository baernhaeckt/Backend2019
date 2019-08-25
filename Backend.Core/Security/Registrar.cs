using Backend.Core.Security.Abstraction;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.Core.Security
{
    public static class Registrar
    {
        public static IServiceCollection AddFeatureLogin(this IServiceCollection services)
        {
            services.AddSingleton<IPasswordStorage, HmacSha256PasswordStorage>();
            services.AddSingleton<ISecurityTokenFactory, JwtSecurityTokenFactory>();
            services.AddSingleton<IPaswordGenerator, StaticPasswordGenerator>();
            services.AddSingleton<ISecurityKeyProvider, SymmetricSecurityKeyProvider>();

            return services;
        }
    }
}
