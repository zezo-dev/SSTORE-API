using Microsoft.OpenApi.Models;

namespace STORE.Api.Extensions
{
    public static class SwaggerServiceExtention
    {
        public static IServiceCollection AddSwaggerDocumentation (this IServiceCollection services)
        {
            services.AddSwaggerGen (options =>
            {
                options.SwaggerDoc("v1",new OpenApiInfo {Title="StorteAPI", Version = "v1" });


                var securityScheme = new OpenApiSecurityScheme {
                 
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorisation",
                    In = ParameterLocation.Header,
                    Type =SecuritySchemeType.ApiKey,
                    Reference = new OpenApiReference { 
                    Type = ReferenceType.SecurityScheme,
                    Id ="bearer"
                    }

                };

                options.AddSecurityDefinition("bearer",securityScheme);


                var securityRequirements = new OpenApiSecurityRequirement
                {
                    {securityScheme , new [ ] {"bearer"} }
                };

                options.AddSecurityRequirement(securityRequirements);

            });
        
        return services;
        }

    }
}
