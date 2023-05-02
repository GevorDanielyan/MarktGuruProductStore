using Application.Services;
using FluentMigrator.Runner;
using Infrastructure.Migrations;
using Infrastructure.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddProductService(this IServiceCollection services)
        {
            services.AddScoped<IDbService, DbService>();
            services.AddScoped<IProductService, ProductService>();

            return services;
        }

        public static IServiceCollection AddFluentMigratorServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                .AddPostgres()
                .WithGlobalConnectionString(configuration.GetConnectionString("DefaultConnection"))
                .ScanIn(typeof(Migration202301302120).Assembly).For.Migrations())
                .AddLogging(lb => lb.AddFluentMigratorConsole());

            return services;

        }

        public static IApplicationBuilder Migrate(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var migrator = scope.ServiceProvider.GetService<IMigrationRunner>();
            migrator?.ListMigrations();
            migrator?.MigrateUp();
            return app;
        }
    }
}
