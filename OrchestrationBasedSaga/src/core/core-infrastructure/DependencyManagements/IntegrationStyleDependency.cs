using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace core_infrastructure.DependencyManagements
{
    public static class IntegrationStyleDependency
    {
        public static IServiceCollection AddAsyncIntegrationStyleDependency(this IServiceCollection services, Action<Options> options)
        {
            var dependencyOptions = new Options();
            options(dependencyOptions);

            services.AddMassTransit(config =>
            {
                if (dependencyOptions.SagaRegistrationSettings != null)
                {
                    dependencyOptions.SagaRegistrationSettings(config);
                }

                dependencyOptions.Consumers.ForEach(consumer =>
                {
                    if (!consumer.isSagaConsumer)
                        config.AddConsumer(consumer.ConsumerType);
                });

                config.UsingRabbitMq((cxt, cfg) =>
                {
                    cfg.Host(new Uri(dependencyOptions.RootUri), x =>
                    {
                        x.Username(dependencyOptions.UserName);
                        x.Password(dependencyOptions.Password);
                    });

                    dependencyOptions.Consumers.ForEach(consumer =>
                    {
                        cfg.ReceiveEndpoint(consumer.QueueName, ep =>
                        {
                            ep.UseMessageRetry(x => x.Interval(dependencyOptions.MaxRetryCount, TimeSpan.FromMilliseconds(dependencyOptions.RetryAttemptInterval)));
                            if (!consumer.isSagaConsumer)
                            {
                                ep.ConfigureConsumer(cxt, consumer.ConsumerType);
                            }
                            else
                            {
                                ep.ConfigureSaga(cxt, consumer.ConsumerType);
                            }
                        });
                    });
                });
            }); 

            return services;
        }

        public sealed class Options
        {
            public string RootUri { get; set; }
            public string UserName { get; set; }
            public string Password { get; set; }
            public int MaxRetryCount { get; set; } = 3;
            public int RetryAttemptInterval { get; set; } = 10000; //MilliSeconds
            public List<Consumer> Consumers { get; set; } = new List<Consumer>();
            public Action<IBusRegistrationConfigurator> SagaRegistrationSettings { get; set; }
        }

        public sealed class Consumer
        {
            public string QueueName { get; set; }
            public Type ConsumerType { get; set; }
            public bool isSagaConsumer { get; set; }
        }
    }
}
