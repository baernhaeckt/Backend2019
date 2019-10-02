using System.Diagnostics.CodeAnalysis;
using Backend.Core.Features.Baseline;
using Backend.Core.Features.Friendship;
using Backend.Core.Features.Newsfeed;
using Backend.Core.Features.Newsfeed.Hubs;
using Backend.Core.Features.Partner;
using Backend.Core.Features.Points;
using Backend.Core.Features.Quiz;
using Backend.Core.Features.UserManagement;
using Backend.Infrastructure.Email;
using Backend.Infrastructure.Geolocation;
using Backend.Infrastructure.Persistence;
using Backend.Web.Middleware;
using Backend.Web.Setup;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Backend.Web
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        private readonly IHostEnvironment _hostEnvironment;

        public Startup(IConfiguration configuration, IHostEnvironment hostEnvironment)
        {
            _configuration = configuration;
            _hostEnvironment = hostEnvironment;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplicationInsightsTelemetry();

            services.AddApiDocumentation();
            services.AddBus(options => options.UseModel());
            services.AddMvcWithCors();
            services.AddJwtAuthentication();

            // Infrastructure
            services.AddMongoDbPersistence(_configuration);
            services.AddInfrastructureEmail(_configuration);
            services.AddGeolocation(_configuration);

            // Features
            services.AddFeatureUserManagement(_hostEnvironment);
            services.AddFeatureBaseline();
            services.AddFeatureFriendship();
            services.AddFeatureNewsfeed();
            services.AddFeaturePartner();
            services.AddFeaturePoints();
            services.AddFeatureQuiz();

            services.AddHostedService<StartupTaskRunner>();
        }

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Framework demand.")]
        public void Configure(IApplicationBuilder app, IHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware(typeof(ErrorHandlingMiddleware));

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Leaf API V1");
            });

            app.UseCors(x =>
                x.AllowAnyMethod()
                    .WithOrigins("http://localhost:8080", "https://baernhaeckt.z16.web.core.windows.net")
                    .AllowAnyHeader()
                    .AllowCredentials());

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers().RequireAuthorization();
                endpoints.MapHub<NewsfeedHub>("/newsfeed");
            });
        }
    }
}