using Backend.Infrastructure.Abstraction.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.Infrastructure.Persistence
{
    public static class Registrar
    {
        public static IServiceCollection AddMongoDbPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MongoDbOptions>(configuration.GetSection(nameof(MongoDbOptions)));

            services.AddSingleton<DbContextFactory>();
            services.AddScoped<IUnitOfWork, MongoDbUnitOfWork>();
            services.AddSingleton<IReader, MongoDbReader>();
            services.AddSingleton<IIndexCreator, IndexCreator>();

            return services;
        }
    }
}