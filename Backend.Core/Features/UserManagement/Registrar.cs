using Backend.Core.Extensions;
using Backend.Core.Features.UserManagement.Commands;
using Backend.Core.Features.UserManagement.Controllers;
using Backend.Core.Features.UserManagement.Data;
using Backend.Core.Features.UserManagement.Data.Testing;
using Backend.Core.Features.UserManagement.EventSubscriber;
using Backend.Core.Features.UserManagement.Queries;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Silverback.Messaging.Subscribers;

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
            services.AddTransient<ISubscriber, UserRegisteredEventSubscriber>();

            // CommandHandlers
            services.AddTransient<ISubscriber, RegisterUserCommandHandler>();
            services.AddTransient<ISubscriber, ChangePasswordCommandHandler>();
            services.AddTransient<ISubscriber, UpdateProfileCommandHandler>();

            // QueryHandlers
            services.AddTransient<ISubscriber, UserProfileQueryHandler>();
            services.AddTransient<ISubscriber, EmailRegisteredQueryHandler>();
            services.AddTransient<ISubscriber, SecurityTokenForUserQueryHandler>();
            services.AddTransient<ISubscriber, SignInQueryHandler>();

            // Data setup
            services.AddStartupTask<SetIndexOnEmailStartupTask>();
            if (hostEnvironment.IsDevelopment())
            {
                services.AddStartupTask<GenerateUsersStartupTask>();
            }

            services.AddStartupTask<AdminUsersStartupTask>();

            return services;
        }
    }
}