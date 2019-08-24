using Backend.Core.Newsfeed;
using Backend.Web.Setup;
using Backend.Web.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
            services.AddApiDocumentation();
            services.AddJwtAuthentication();
            services.AddNewsfeed();
            services.AddServices();
            services.AddMongoDb(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

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

            app.UseAuthentication();

            app.UseSignalR(routes =>
            {
                routes.MapHub<NewsfeedHub>("/newsfeed");
            });

            app.UseMiddleware(typeof(ErrorHandlingMiddleware));
            app.UseMvc();
        }
    }
}
