using Backend.Core.Extensions;
using Backend.Core.Features.Quiz.Controllers;
using Backend.Core.Features.Quiz.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.Core.Features.Quiz
{
    public static class Registrar
    {
        public static IServiceCollection AddFeatureQuiz(this IServiceCollection services)
        {
            // Controllers
            services.AddScoped<QuizController>();

            // Services
            services.AddScoped<QuizService>();

            // StartupTasks
            services.AddStartupTask<GenerateQuizQuestionsStartupTask>();

            return services;
        }
    }
}
