using System.Reflection;

using Microsoft.OpenApi.Models;

namespace RestApi.Extensions
{
    public static class ServiceCollectionExt
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.ConfigureSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1.0",
                    Title = $"APP API",
                    Description = $"Версия сборки v.{Assembly.GetExecutingAssembly().GetName().Version}",
                });


                options.SupportNonNullableReferenceTypes();
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });

            return services;
        }
    }
}
