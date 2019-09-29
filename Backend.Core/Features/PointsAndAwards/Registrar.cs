using Backend.Core.Features.PointsAndAwards.Controllers;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.Core.Features.PointsAndAwards
{
    public static class Registrar
    {
        public static IServiceCollection AddFeaturePointsAndAwards(this IServiceCollection services)
        {
            // Controllers
            services.AddScoped<AwardsController>();
            services.AddScoped<PointsController>();

            // Services
            services.AddScoped<AwardService>();
            services.AddScoped<PointService>();

            return services;
        }
    }
}
