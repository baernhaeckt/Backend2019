using Microsoft.Extensions.DependencyInjection;

namespace Backend.Core.Security
{
    public static class Registrar
    {
        public static IServiceCollection AddFeatureLogin(this IServiceCollection services)
        {
            services.AddSingleton<IPasswordStorage, HmacSha256PasswordStorage>();
            services.AddSingleton<ISecurityTokenFactory, JwtSecurityTokenFactory>();

            return services;
        }
    }
}
