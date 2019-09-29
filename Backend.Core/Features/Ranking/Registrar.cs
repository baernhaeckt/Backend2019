using Backend.Core.Features.Ranking.Controllers;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.Core.Features.Ranking
{
    public static class Registrar
    {
        public static IServiceCollection AddFeatureRanking(this IServiceCollection services)
        {
            // Controllers
            services.AddScoped<RankingsController>();

            return services;
        }
    }
}
