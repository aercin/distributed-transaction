using core_application.Interfaces;
using core_domain.Interfaces;
using core_infrastructure.DependencyManagements;
using core_infrastructure.persistence;
using core_infrastructure.Services;
using infrastructure.Persistence;
using infrastructure.Services;
using MassTransit;
using MassTransit.EntityFrameworkCoreIntegration;
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

            services.AddSingleton<IOutboxMessageRepository, OutboxMessageRepository<OrchestratorDbContext>>();
            services.AddSingleton<ISystemClock, SystemClock>();

            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            services.AddDbContext<OrchestratorDbContext>(options => options.UseNpgsql(dependencyOptions.Value.Configuration.GetConnectionString("OrchestratorDb")), contextLifetime: ServiceLifetime.Singleton);

            services.AddAsyncIntegrationStyleDependency(cfg =>
            {
                cfg.RootUri = dependencyOptions.Value.Configuration.GetValue<string>("RabbitMQ:RootUri");
                cfg.UserName = dependencyOptions.Value.Configuration.GetValue<string>("RabbitMQ:UserName");
                cfg.Password = dependencyOptions.Value.Configuration.GetValue<string>("RabbitMQ:Password");
                cfg.Consumers = dependencyOptions.Value.Consumers.Select(x => new IntegrationStyleDependency.Consumer
                {
                    QueueName = x.QueueName,
                    ConsumerType = x.StateInstanceType,
                    isSagaConsumer = true
                }).ToList();
                cfg.SagaRegistrationSettings = busConfigurator =>
                                               {
                                                   busConfigurator.AddSagaStateMachine<OrderStateMachine, OrderStateInstance>()
                                                                  .EntityFrameworkRepository(options =>
                                                                  {
                                                                      options.ExistingDbContext<OrchestratorDbContext>();
                                                                      options.LockStatementProvider = new PostgresLockStatementProvider();
                                                                  });
                                               };
            });

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
            public Type StateInstanceType { get; set; }
        }
    }
}
