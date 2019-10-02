using Backend.Core.Features.Points.Controllers;
using Backend.Core.Features.Points.EventHandler;
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

            // EventHandlers
            services.AddScoped<ISubscriber, PartnerTokenRegisteredEventHandler>();
            services.AddScoped<ISubscriber, QuizQuestionCorrectAnsweredEventHandler>();

            return services;
        }
    }
}