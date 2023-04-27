using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace RestApi.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSwaggerServices(this IServiceCollection services)
        {
            _ = services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Products Web API",
                    Description = "Web API for Products."
                });

                AddSwaggerXml(options);
            });
            return services;
        }

        private static void AddSwaggerXml(SwaggerGenOptions options)
        {
            var xmlFiles = Directory.GetFiles(AppContext.BaseDirectory, "*.xml");
            foreach (var xmlFile in xmlFiles)
            {
                options.IncludeXmlComments(xmlFile);
            }
        }
    }
}
