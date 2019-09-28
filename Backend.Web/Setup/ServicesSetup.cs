using Backend.Core.Newsfeed;
using Backend.Core.Security;
using Backend.Core.Seed;
using Backend.Core.Seed.Testing;
using Backend.Core.Services;
using Backend.Core.Services.Widgets;
using Backend.Web.StartupTask;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Backend.Web.Setup
{
    public static class ServicesSetup
    {
        public static void AddServices(this IServiceCollection services, IHostEnvironment hostEnvironment)
        {
            services.AddFeatureLogin(hostEnvironment);

            services.AddScoped<FriendsService>();
            services.AddScoped<PointService>();
            services.AddScoped<TokenService>();
            services.AddScoped<UserService>();
            services.AddScoped<SufficientTypeService>();
            services.AddScoped<AwardService>();
            services.AddScoped<IQuizService, QuizService>();
            services.AddSingleton<IEventStream, SignalREventStream>();

            // Data seeding
            if (hostEnvironment.IsDevelopment())
            {
                services.AddStartupTask<GenerateUsersStartupTask>();
            }

            services.AddStartupTask<GenerateQuizQuestionsStartupTask>();
            services.AddStartupTask<GenerateSufficientTypesStartupTask>();
            services.AddStartupTask<AdminUsersStartupTask>();
        }
    }
}
