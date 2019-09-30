using Backend.Core.Features.Points.Commands;
using Backend.Core.Features.Points.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Silverback.Messaging.Subscribers;

namespace Backend.Core.Features.Points
{
    public static class Registrar
    {
        public static IServiceCollection AddFeaturePoints(this IServiceCollection services)
        {
            // Controllers
            services.AddScoped<PointsController>();
            services.AddScoped<RankingsController>();

            // Services
            services.AddScoped<PointService>();

            // Commands
            services.AddScoped<ISubscriber, PointsForTokenRewardCommandHandler>();

            return services;
        }
    }
}