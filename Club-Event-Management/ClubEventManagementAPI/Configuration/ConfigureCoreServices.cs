using ApplicationCore.Interfaces.Repository;
using ApplicationCore.Models;
using ApplicationCore.Services;
using Infrastructure;
using Infrastructure.Repository;
using Infrastructure.Services.ClubProfileServices.Implementation;
using Infrastructure.Services.EventPostServices.Implementation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace ClubEventManagementAPI.Configuration
{
    public static class ConfigureCoreServices
    {
        public static IServiceCollection AddCoreServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IAccountRepository, AccountRepository>();
            services.AddSingleton<IAuthorizationRepository, AuthorizationRepository>();
            services.AddDbContext<ClubEventManagementContext>(
                options => options.UseSqlServer("Data Source=5CD1218GHG\\SQLEXPRESS;Initial Catalog=ClubEventManagement;Integrated Security=True"));
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ClubEventManagementAPI", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });
            return services;

        }
    }
}
