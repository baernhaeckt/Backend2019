﻿using Backend.Infrastructure.Geolocation.Abstraction;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Backend.Infrastructure.Geolocation
{
    public static class Registrar
    {
        public static IServiceCollection AddGeolocation(this IServiceCollection services, IConfiguration configuration, IHostEnvironment hostEnvironment)
        {
            services.Configure<GeocodingOptions>(configuration.GetSection(nameof(GeocodingOptions)));

            if (hostEnvironment.IsDevelopment())
            {
                services.AddSingleton<IGeocodingService, BingGeocodingService>();
            }
            else
            {
                services.AddSingleton<IGeocodingService, BingGeocodingService>();
            }

            return services;
        }
    }
}