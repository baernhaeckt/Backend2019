using Backend.Core;
using Backend.Core.Newsfeed;
using Backend.Core.Security;
using Backend.Core.Services;
using Backend.Core.Services.Widgets;
using Backend.Web.StartupTask;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.Web.Setup
{
    public static class ServicesSetup
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddFeatureLogin();

            services.AddScoped<FriendsService>();
            services.AddScoped<PointService>();
            services.AddScoped<TokenService>();
            services.AddScoped<UserService>();
            services.AddScoped<SufficientTypeService>();
            services.AddScoped<AwardService>();
            services.AddScoped<IQuizService, QuizService>();
            services.AddSingleton<IEventStream, SignalREventStream>();

            services.AddStartupTask<GenerateQuizQuestionsStartupTask>();
            services.AddStartupTask<GenerateSufficientTypesStartupTask>();
        }
    }
}
