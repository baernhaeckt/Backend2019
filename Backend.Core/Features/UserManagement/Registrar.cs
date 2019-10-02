﻿using Backend.Core.Extensions;
using Backend.Core.Features.UserManagement.Commands;
using Backend.Core.Features.UserManagement.Controllers;
using Backend.Core.Features.UserManagement.Data;
using Backend.Core.Features.UserManagement.Data.Testing;
using Backend.Core.Features.UserManagement.EventSubscriber;
using Backend.Core.Features.UserManagement.Queries;
using Backend.Core.Features.UserManagement.Security;
using Backend.Core.Features.UserManagement.Security.Abstraction;
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

            // Services
            services.AddScoped<UserService>();

            // Subscribers
            services.AddTransient<ISubscriber, UserRegisteredEventSubscriber>();

            // CommandHandlers
            services.AddTransient<ISubscriber, RegisterUserCommandHandler>();
            services.AddTransient<ISubscriber, ChangePasswordCommandHandler>();
            services.AddTransient<ISubscriber, UpdateProfileCommandHandler>();

            // QueryHandlers
            services.AddTransient<ISubscriber, UserProfileQueryHandler>();

            // Data setup
            services.AddStartupTask<SetIndexOnEmailStartupTask>();
            if (hostEnvironment.IsDevelopment())
            {
                services.AddStartupTask<GenerateUsersStartupTask>();
            }

            services.AddStartupTask<AdminUsersStartupTask>();

            // Security Utilities
            services.AddSingleton<IPasswordStorage, HmacSha512PasswordStorage>();
            services.AddSingleton<ISecurityTokenFactory, JwtSecurityTokenFactory>();
            services.AddSingleton<ISecurityKeyProvider, SymmetricSecurityKeyProvider>();
            if (hostEnvironment.IsDevelopment())
            {
                services.AddSingleton<IPasswordGenerator, StaticPasswordGenerator>();
            }
            else
            {
                services.AddSingleton<IPasswordGenerator, RandomPasswordGenerator>();
            }

            return services;
        }
    }
}