using Backend.Core.Features.Points.Controllers;
using Backend.Core.Features.Points.EventSubscribers;
using Backend.Core.Features.Points.Queries;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.Core.Features.Points
{
    public static class Registrar
    {
        public static IServiceCollection AddFeaturePoints(this IServiceCollection services)
        {
            // Controllers
            services.AddScoped<PointsController>();
            services.AddScoped<RankingsController>();

            // QueryHandlers
            services.AddScopedSubscriber<RankingQueryHandler>();
            services.AddScopedSubscriber<PointHistoryForUserQueryHandler>();

            // EventHandlers
            services.AddScopedSubscriber<PartnerTokenRegisteredEventHandler>();
            services.AddScopedSubscriber<QuizQuestionCorrectAnsweredEventHandler>();

            return services;
        }
    }
}