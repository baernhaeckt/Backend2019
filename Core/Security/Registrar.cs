using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Security
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
