using ApplicationCore.Interfaces.Services;
using ApplicationCore.Services;
using AutoMapper;
using ClubEventManagementAPI.Mapping;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ClubEventManagementAPI.Configuration
{
    public static class ConfigureWebServices
    {
        public static IServiceCollection AddWebServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped(typeof(IAccountService), typeof(AccountService));
            services.AddScoped(typeof(IAuthorizationService), typeof(AuthorizationService));
            services.AddAutoMapper(typeof(MappingProfile));
            return services;
        }
    }
}
