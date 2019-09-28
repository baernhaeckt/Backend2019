using Backend.Database.Abstraction;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.Database.Framework
{
    public static class Registrar
    {
        public static IServiceCollection AddMongoDbPersistance(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MongoDbOptions>(configuration.GetSection(nameof(MongoDbOptions)));

            services.AddSingleton<DbContextFactory>();
            services.AddScoped<IUnitOfWork, MongoDbUnitOfWork>();
            services.AddSingleton<IReader, MongoDbReader>();

            return services;
        }
    }
}
