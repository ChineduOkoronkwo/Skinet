using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace services.Extensions
{
    public static class SwaggerServiceExtensions
    {
        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services) {
            return 
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "SkiNet ", Version = "v1" });
            });
        }

        public static IApplicationBuilder UserSwaggerDocumentation(this IApplicationBuilder app) 
        {
            app.UseSwagger();

            app.UseSwaggerUI(c => {c
                .SwaggerEndpoint("/swagger/v1/swagger.json", "SkiNet Api v1");});

            return app;
        }
    }    
}