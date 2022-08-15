using core_application.Interfaces;
using core_domain.Interfaces;
using core_infrastructure.DependencyManagements;
using core_infrastructure.persistence;
using core_infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace core_infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCoreInfrastructure<T>(this IServiceCollection services, Action<DependencyOptions> options) where T : DbContext
        {
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IOutboxMessageRepository, OutboxMessageRepository<T>>();
            services.AddScoped<ISystemClock, SystemClock>();
            services.AddScoped<IEventDispatcher, EventDispatcher<T>>();

            services.AddOptions<DependencyOptions>().Configure(options);

            var dependencyOptions = services.BuildServiceProvider().GetService<IOptions<DependencyOptions>>();

            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            services.AddDbContext<T>(options => options.UseNpgsql(dependencyOptions.Value.ConnectionString));

            services.AddOptions<IntegrationStyleDependency.Options>().Configure(dependencyOptions.Value.IntegrationStyleConfig);
            var integrationStyleOptions = services.BuildServiceProvider().GetService<IOptions<IntegrationStyleDependency.Options>>();
            services.AddAsyncIntegrationStyleDependency(cfg =>
            {
                cfg.RootUri = integrationStyleOptions.Value.RootUri;
                cfg.UserName = integrationStyleOptions.Value.UserName;
                cfg.Password = integrationStyleOptions.Value.Password;
                cfg.Consumers = integrationStyleOptions.Value.Consumers;
            });

            services.AddOptions<DistributedCacheDependency.Options>().Configure(dependencyOptions.Value.CacheConfig);
            var distributedCacheOptions = services.BuildServiceProvider().GetService<IOptions<DistributedCacheDependency.Options>>();
            services.AddDistributedCacheDependency(cfg =>
            {
                cfg.Endpoints = distributedCacheOptions.Value.Endpoints;
                cfg.Password = distributedCacheOptions.Value.Password;
                cfg.Database = distributedCacheOptions.Value.Database;
            });

            return services;
        }

        public sealed class DependencyOptions
        {
            public string ConnectionString { get; set; }
            public Action<IntegrationStyleDependency.Options> IntegrationStyleConfig { get; set; }
            public Action<DistributedCacheDependency.Options> CacheConfig { get; set; }
        }
    }
}
