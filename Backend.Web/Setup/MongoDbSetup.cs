using Backend.Core.Features.UserManagement.Data;
using Backend.Core.Startup;
using Backend.Database.Framework;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.Web.Setup
{
    public static class MongoDbSetup
    {
        public static void AddMongoDb(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMongoDbPersistance(configuration);
            services.AddSingleton<IStartupTask, MongoDbSetupStartupTask>();
        }
    }
}
