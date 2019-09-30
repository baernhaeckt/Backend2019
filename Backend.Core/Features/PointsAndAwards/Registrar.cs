using Backend.Core.Features.PointsAndAwards.Commands;
using Backend.Core.Features.PointsAndAwards.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Silverback.Messaging.Subscribers;

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

            // Commands
            services.AddScoped<ISubscriber, TokenRewardCommandHandler>();

            return services;
        }
    }
}