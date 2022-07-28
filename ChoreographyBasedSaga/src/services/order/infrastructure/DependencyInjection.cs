﻿using core_application.Interfaces;
using core_infrastructure;
using core_infrastructure.DependencyManagements;
using domain.Interfaces;
using infrastructure.persistence;
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

            services.AddCoreInfrastructure<OrderDbContext>(cfg =>
            {
                cfg.ConnectionString = dependencyOptions.Value.Configuration.GetConnectionString("OrderDb");
                cfg.CacheConfig = (cacheCfg) =>
                {
                    cacheCfg.Endpoints = dependencyOptions.Value.Configuration.GetValue<string>("Redis:Endpoints");
                    cacheCfg.Password = dependencyOptions.Value.Configuration.GetValue<string>("Redis:Password");
                    cacheCfg.Database = dependencyOptions.Value.Configuration.GetValue<int>("Redis:Database");
                };
                cfg.IntegrationStyleConfig = (integrationStyleCfg) =>
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

            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddSingleton<IDomainEventToMessageMapper, DomainEventToMessageMapper>();
            //services.AddDbContext<OrderDbContext>(options => options.UseInMemoryDatabase(databaseName: "OrderSubDomain")); 

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
