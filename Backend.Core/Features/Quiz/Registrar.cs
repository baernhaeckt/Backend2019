using Backend.Core.Extensions;
using Backend.Core.Features.Quiz.Commands;
using Backend.Core.Features.Quiz.Controllers;
using Backend.Core.Features.Quiz.Data;
using Backend.Core.Features.Quiz.Queries;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.Core.Features.Quiz
{
    public static class Registrar
    {
        public static IServiceCollection AddFeatureQuiz(this IServiceCollection services)
        {
            // Controllers
            services.AddScoped<QuizController>();

            // CommandHandlers
            services.AddScopedSubscriber<AnswerQuizQuestionCommandHandler>();

            // QueryHandlers
            services.AddScopedSubscriber<QuizQuestionForTodayQueryHandler>();

            // StartupTasks
            services.AddStartupTask<GenerateQuizQuestionsStartupTask>();

            return services;
        }
    }
}