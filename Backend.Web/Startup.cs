using AspNetCore.MongoDB;
using Backend.Core;
using Backend.Core.Security;
using Backend.Core.Services;
using Backend.Core.Services.Awards;
using Backend.Web.StartupTask;
using Backend.Web.Setup;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using System.Collections.Generic;
using System.Linq;
using Backend.Core.Hubs;

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

            services.Configure<MongoDBOption>(Configuration.GetSection("MongoDBOption"))
                .AddMongoDatabase();
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
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseCors(x =>
                    x.AllowAnyMethod()
                    .WithOrigins("http://localhost:8080", "https://baernhaeckt.z16.web.core.windows.net")
                    .AllowAnyHeader()
                    .AllowCredentials());

            app.UseSignalR(routes =>
            {
                routes.MapHub<NewsfeedHub>("/newsfeed");
            });

            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
