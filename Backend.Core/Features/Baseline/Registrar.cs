using Backend.Core.Extensions;
using Backend.Core.Features.Baseline.Controllers;
using Backend.Core.Features.Baseline.Data;
using Microsoft.Extensions.DependencyInjection;

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

            services.AddStartupTask<GenerateSufficientTypesStartupTask>();

            return services;
        }
    }
}