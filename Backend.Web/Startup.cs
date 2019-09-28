using Backend.Core.Newsfeed;
using Backend.Web.Middleware;
using Backend.Web.Setup;
using Backend.Web.StartupTask;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Backend.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvcWithCors();
            //services.AddApiDocumentation(); Wait for Swagger v5
            services.AddJwtAuthentication();
            services.AddNewsfeed();
            services.AddServices();
            services.AddMongoDb(Configuration);

            services.AddHostedService<StartupTaskRunner>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware(typeof(ErrorHandlingMiddleware));

            // Wait for Swagger v5
            //app.UseSwagger();
            //app.UseSwaggerUI(c =>
            //{
            //    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Leaf API V1");
            //});

            app.UseCors(x =>
                    x.AllowAnyMethod()
                    .WithOrigins("http://localhost:8080", "https://baernhaeckt.z16.web.core.windows.net")
                    .AllowAnyHeader()
                    .AllowCredentials());

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseRouting();
            app.UseEndpoints(endpoints => 
            {
                endpoints.MapControllers();
                endpoints.MapHub<NewsfeedHub>("/newsfeed");
            });
        }
    }
}
