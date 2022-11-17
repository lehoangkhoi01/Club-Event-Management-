﻿using ApplicationCore;
using ApplicationCore.Services;
using ApplicationCore.Interfaces.Services;
using AutoMapper;
using ClubEventManagementAPI.Mapping;
using Infrastructure.Services.AccountService.Implementation;
using Infrastructure.Services.ClubProfileServices.Implementation;
using Infrastructure.Services.EventPostServices.Implementation;
using Infrastructure.Services.EventServices.Implementation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ClubEventManagementAPI.Helpers;
using Infrastructure.Services.FirebaseServices.NotificationService;
using Infrastructure.Services.BackgroundService;

namespace ClubEventManagementAPI.Configuration
{
    public static class ConfigureWebServices
    {
        public static IServiceCollection AddWebServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped(typeof(IAccountService), typeof(ApplicationCore.Services.AccountService));
            services.AddScoped(typeof(IAuthorizationService), typeof(AuthorizationService));
            services.AddScoped<EventPostService>();
            services.AddScoped<EventActivityService>();
            services.AddScoped<ClubProfileService>();
            services.AddScoped<Infrastructure.Services.AccountService.Implementation.AccountService>();
            services.AddScoped<EventService>();
            services.AddScoped<UserContextService>();
            services.AddScoped<NotificationService>();
            services.AddHostedService<TimedHostedService>();
            //services.AddScoped(typeof(IPostService), typeof(PostService));
            services.AddAutoMapper(typeof(MappingProfile));
            return services;
        }
    }
}
