using Backend.Core.Extensions;
using Backend.Core.Features.Awards.EventHandler;
using Backend.Core.Features.Baseline.Controllers;
using Backend.Core.Features.Baseline.Data;
using Microsoft.Extensions.DependencyInjection;
using Silverback.Messaging.Subscribers;

namespace Backend.Core.Features.Baseline
{
    public static class Registrar
    {
        public static IServiceCollection AddFeatureBaseline(this IServiceCollection services)
        {
            // Controllers
            services.AddScoped<SufficientTypeController>();

            // Services
            services.AddScoped<SufficientTypeService>();

            // EventHandlers
            services.AddScoped<ISubscriber, UserNewPointsEventHandler>();

            // Startup Tasks
            services.AddStartupTask<GenerateSufficientTypesStartupTask>();

            return services;
        }
    }
}