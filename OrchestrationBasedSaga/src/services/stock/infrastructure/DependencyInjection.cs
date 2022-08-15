using core_application.Interfaces;
using core_infrastructure;
using core_infrastructure.DependencyManagements;
using domain.Interfaces;
using infrastructure.Persistence;
using infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, Action<DependencyOptions> options)
        {
            services.AddOptions<DependencyOptions>().Configure(options);
            var dependencyOptions = services.BuildServiceProvider().GetService<IOptions<DependencyOptions>>();
             
            services.AddCoreInfrastructure<StockDbContext>(x =>
            {
                x.ConnectionString = dependencyOptions.Value.Configuration.GetConnectionString("StockDb");
                x.CacheConfig = (DistributedCacheDependency.Options cacheCfg) =>
                {
                    cacheCfg.Endpoints = dependencyOptions.Value.Configuration.GetValue<string>("Redis:Endpoints");
                    cacheCfg.Password = dependencyOptions.Value.Configuration.GetValue<string>("Redis:Password");
                    cacheCfg.Database = dependencyOptions.Value.Configuration.GetValue<int>("Redis:Database");
                };
                x.IntegrationStyleConfig = (IntegrationStyleDependency.Options integrationStyleCfg) =>
                {
                    integrationStyleCfg.RootUri = dependencyOptions.Value.Configuration.GetValue<string>("RabbitMQ:RootUri");
                    integrationStyleCfg.UserName = dependencyOptions.Value.Configuration.GetValue<string>("RabbitMQ:UserName");
                    integrationStyleCfg.Password = dependencyOptions.Value.Configuration.GetValue<string>("RabbitMQ:Password");
                    integrationStyleCfg.Consumers = dependencyOptions.Value.Consumers.Select(y => new IntegrationStyleDependency.Consumer
                    {
                        QueueName = y.QueueName,
                        ConsumerType = y.ConsumerType
                    }).ToList();
                };
            });

            services.AddScoped<IStockRepository, StockRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddSingleton<IDomainEventToMessageMapper, DomainEventToMessageMapper>(); 

            return services;
        }

        public sealed class DependencyOptions
        {
            public IConfiguration Configuration { get; set; }

            public List<Consumer> Consumers { get; set; }
        }

        public sealed class Consumer
        {
            public string QueueName { get; set; }
            public Type ConsumerType { get; set; }
        }
    }
}
