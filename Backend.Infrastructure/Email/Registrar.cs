using Backend.Infrastructure.Abstraction.Email;
using Backend.Infrastructure.Abstraction.Hosting;
using Backend.Infrastructure.Email.Fakes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Backend.Infrastructure.Email
{
    public static class Registrar
    {
        public static IServiceCollection AddInfrastructureEmail(this IServiceCollection services, IConfiguration configuration, IHostEnvironment hostEnvironment)
        {
            services.Configure<SendGridOptions>(configuration.GetSection(nameof(SendGridOptions)));

            if (hostEnvironment.IsIntegrationTestOrDevelopment())
            {
                services.AddSingleton<IEmailService, InMemoryEmailService>();
            }
            else
            {
                services.AddSingleton<IEmailService, SendGridEmailService>();
            }

            return services;
        }
    }
}