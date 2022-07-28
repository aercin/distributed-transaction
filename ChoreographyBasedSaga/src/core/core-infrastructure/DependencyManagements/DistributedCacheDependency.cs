using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace core_infrastructure.DependencyManagements
{
    public static class DistributedCacheDependency
    {
        public static IServiceCollection AddDistributedCacheDependency(this IServiceCollection services, Action<Options> options)
        {
            var dependencyOptions = new Options();
            options(dependencyOptions);

            var redisConfigOption = ConfigurationOptions.Parse(dependencyOptions.Endpoints);
            redisConfigOption.Password = dependencyOptions.Password;
            redisConfigOption.DefaultDatabase = dependencyOptions.Database;

            services.AddStackExchangeRedisCache(options =>
            {
                options.ConfigurationOptions = redisConfigOption;
            });

            return services;
        }

        public sealed class Options
        {
            public string Endpoints { get; set; } //Host1:Port1,Host2:Port2 vb.
            public string Password { get; set; }
            public int Database { get; set; }
        }
    }
}
