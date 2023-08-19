using Infrastructure.Transaction.Services.Workers;
using Microsoft.OpenApi.Models;

namespace PaymentGateway.Api.Config
{
    // https://www.infoworld.com/article/3650668/implement-authorization-for-swagger-in-aspnet-core-6.html
    public static class SwaggerServiceRegistration
    {
        public static IServiceCollection ConfigureSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo { Title = "Payment Gateway API", Version = "v1" });
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid Bearer token. In the format 'Bearer ***'",
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
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
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
