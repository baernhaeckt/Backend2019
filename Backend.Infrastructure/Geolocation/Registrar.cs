using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.Infrastructure.Geolocation
{
    public static class Registrar
    {
        public static IServiceCollection AddGeolocation(this IServiceCollection services, IConfiguration configuration)
        {
            //services.Configure<MongoDbOptions>(configuration.GetSection(nameof(MongoDbOptions)));

            services.AddSingleton<IGeolocationService, FakeGeolocationService>();

            return services;
        }
    }
}