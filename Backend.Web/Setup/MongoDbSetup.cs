using AspNetCore.MongoDB;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.Web.Setup
{
    public static class MongoDbSetup
    {
        public static void AddMongoDb(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MongoDBOption>(configuration.GetSection("MongoDBOption"))
                .AddMongoDatabase();
        }
    }
}
