using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace RestApi.Infrastructure.Caching.Extensions
{
    public static class RedisOutputCacheServiceCollectionExtensions
    {
        public static IServiceCollection AddRedisOutputCache(this IServiceCollection services)
        {
            ArgumentNullException.ThrowIfNull(services);

            services.AddOutputCache(options => 
            {
                options.AddBasePolicy(x => x.Tag("tag-all"));
            });

            services.RemoveAll<IOutputCacheStore>();

            services.AddSingleton<IOutputCacheStore, RedisOutputCacheStore>();

            return services;
        }

        public static IServiceCollection AddRedisOutputCache(this IServiceCollection services, Action<OutputCacheOptions> configureOptions)
        {
            ArgumentNullException.ThrowIfNull(services);
            ArgumentNullException.ThrowIfNull(configureOptions);

            services.Configure(configureOptions);
            services.AddOutputCache();

            services.RemoveAll<IOutputCacheStore>();
            services.AddSingleton<IOutputCacheStore, RedisOutputCacheStore>();

            return services;
        }
    }
}
