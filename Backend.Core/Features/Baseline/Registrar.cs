using Backend.Core.Features.Baseline.Controllers;
using Backend.Core.Features.Baseline.Queries;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.Core.Features.Baseline
{
    public static class Registrar
    {
        public static IServiceCollection AddFeatureBaseline(this IServiceCollection services)
        {
            // Controllers
            services.AddScoped<SufficientTypeController>();

            // QueryHandlers
            services.AddScopedSubscriber<AveragePointsPerSufficientTypesQueryHandler>();
            services.AddScopedSubscriber<PointsPerSufficientTypesQueryHandler>();

            return services;
        }
    }
}