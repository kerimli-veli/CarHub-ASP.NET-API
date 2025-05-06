using Microsoft.OpenApi.Models;

namespace CarHub.Api.Security;

public static class AddSwaggerGenService
{
    public static IServiceCollection AddSwaggerService(this IServiceCollection services)
    {
        services.AddSwaggerGen(option =>
        {
            // ✨ ƏSAS Swagger sənədləşmə
            option.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "CarHub API",
                Version = "v1"
            });

            // 🔐 JWT security configuration
            option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });
            option.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                    new string[] {}
                }
            });
        });

        return services;
    }
}
