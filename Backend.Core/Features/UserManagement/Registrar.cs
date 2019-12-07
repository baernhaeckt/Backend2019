using Backend.Core.Extensions;
using Backend.Core.Features.UserManagement.Commands;
using Backend.Core.Features.UserManagement.Controllers;
using Backend.Core.Features.UserManagement.Data;
using Backend.Core.Features.UserManagement.Data.Testing;
using Backend.Core.Features.UserManagement.EventSubscribers;
using Backend.Core.Features.UserManagement.Queries;
using Backend.Core.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Backend.Core.Features.UserManagement
{
    public static class Registrar
    {
        public static IServiceCollection AddFeatureUserManagement(this IServiceCollection services, IHostEnvironment hostEnvironment)
        {
            // Controllers
            services.AddScoped<ProfileController>();
            services.AddScoped<UsersController>();

            // Subscribers
            services.AddScopedSubscriber<UserRegisteredEventSubscriber>();

            // CommandHandlers
            services.AddScopedSubscriber<RegisterUserCommandHandler>();
            services.AddScopedSubscriber<ChangePasswordCommandHandler>();
            services.AddScopedSubscriber<UpdateProfileCommandHandler>();

            // QueryHandlers
            services.AddScopedSubscriber<UserProfileQueryHandler>();
            services.AddScopedSubscriber<EmailRegisteredQueryHandler>();
            services.AddScopedSubscriber<SecurityTokenForUserQueryHandler>();
            services.AddScopedSubscriber<SignInQueryHandler>();

            // Data setup
            services.AddStartupTask<SetIndexOnEmailStartupTask>();
            if (hostEnvironment.IsIntegrationTestOrDevelopment())
            {
                services.AddStartupTask<GenerateUsersStartupTask>();
            }

            services.AddStartupTask<AdminUsersStartupTask>();

            return services;
        }
    }
}