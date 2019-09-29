using Backend.Infrastructure.Email.Abstraction;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.Infrastructure.Email
{
    public static class Registrar
    {
        public static IServiceCollection AddInfrastructureEmail(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<SendGridOptions>(configuration.GetSection(nameof(SendGridOptions)));

            services.AddSingleton<IEmailService, SendGridEmailService>();

            return services;
        }
    }
}